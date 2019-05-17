using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;

namespace MscAudio {

    /// <summary>
    /// 语音识别
    /// </summary>
    public class SpeechRecognition {

        // 识别数据
        private byte[] m_Datas;
        // 控制参数
        private string @params = "sub = iat, domain = iat, language = zh_cn, accent = mandarin, sample_rate = 16000, result_type = plain, result_encoding = utf-8";

        /// <summary>
        /// 识别是否完成
        /// 用于在类外部判断合成是否完成
        /// </summary>
        public bool isDown { get; private set; }
        /// <summary>
        /// 识别信息
        /// 外部访问
        /// </summary>
        public string Data { get; private set; }


        public SpeechRecognition(string path) {

            try {
                m_Datas = File.ReadAllBytes(path);
            } catch (Exception e) {
                Debug.Log(e);
            }

            if (m_Datas == null) Debug.Log("没有读取到数据");
        }


        public SpeechRecognition(byte[] datas) {

            if (datas == null || datas.Length <= 0) {
                Debug.Log("传入字节数据有错");
            }
            m_Datas = datas;
        }


        /// <summary>
        /// 开始识别
        /// </summary>
        /// <param name="grammarList"> 
        /// 1:在线 关键词识别(sub= asr) 传入调用MSPUploadData接口上传关键词的返回值。关键词会永久生效
        /// 2:在线 语法识别(sub= asr)   除上述方法外，也可在此处传入语法字符串指针，并在params参数中添加"grammartype=abnf"或" grammartype=xml"。此方法仅在本次识别有效。
        /// 3;在线 连续语音识别(sub= iat) 此参数设为NULL
        /// 4:离线 此参数设为NULL
        /// </param>
        /// <param name="params">控制参数</param>
        public void Begin() {

            // 登录
            if (!Account.Login()) {
                
                return;
            }

            new Thread(ThreadStart).Start();


        }

        /// <summary>
        ///  识别线程
        /// </summary>
        private void ThreadStart() {

            try {
                ErrorCode errorCode = ErrorCode.MSP_SUCCESS;
                AudioStatus audioStatus = AudioStatus.MSP_AUDIO_SAMPLE_FIRST;
                EpStatus epStatus = EpStatus.MSP_EP_IN_SPEECH;
                RsltStatus rsltStatus = RsltStatus.MSP_REC_STATUS_SUCCESS;

                // 开始
                IntPtr ptr = Msc.QISRSessionBegin(null, @params, out errorCode);

                if (errorCode != ErrorCode.MSP_SUCCESS) {
                    Debug.Log("开始识别出错：（错误码）" + (int) errorCode);
                    Account.Logout();

                    return;
                }

                int maxLength = 20480;
                int tims = m_Datas.Length / maxLength;
                if (( m_Datas.Length % maxLength ) != 0) {
                    tims++;
                }


                for (int i = 0; i < tims; i++) {

                    if (i == 0) audioStatus = AudioStatus.MSP_AUDIO_SAMPLE_FIRST;
                    else if (i == tims - 1) audioStatus = AudioStatus.MSP_AUDIO_SAMPLE_LAST;
                    else audioStatus = AudioStatus.MSP_AUDIO_SAMPLE_CONTINUE;

                    byte[] by = null;
                    if (( m_Datas.Length - maxLength * i ) >= maxLength) {
                        by = new byte[maxLength];
                    } else {
                        by = new byte[( m_Datas.Length - maxLength * i )];
                    }

                    by = m_Datas.Skip(i * maxLength).Take(by.Length).ToArray();

                    ErrorCode errorCode1 = Msc.QISRAudioWrite(ptr, by, (uint) by.Length, audioStatus, out epStatus, out rsltStatus);

                    if (errorCode1 != ErrorCode.MSP_SUCCESS) {
                        Debug.Log("音频数据写入失败:（错误码）" + (int) errorCode1);
                        Account.Logout();

                        return;

                    }
                }

                RsltStatus status = RsltStatus.MSP_REC_STATUS_INCOMPLETE;
                ErrorCode errorCode2 = ErrorCode.MSP_SUCCESS;
                StringBuilder msg = new StringBuilder();

                while (status != RsltStatus.MSP_REC_STATUS_COMPLETE) {

                    IntPtr data = Msc.QISRGetResult(ptr, out status, 5000, out errorCode2);

                    if (errorCode2 != ErrorCode.MSP_SUCCESS) {
                        Debug.Log("获取失败：（错误码）" + (int) errorCode2);
                        Account.Logout();
                        return;
                    }

                    if (data != null) {
                        msg.Append(Marshal.PtrToStringAnsi(data));
                    }
                    Thread.Sleep(200);
                }

                ErrorCode errorCode3 = Msc.QISRSessionEnd(ptr, "识别结束");

                if (errorCode3 != ErrorCode.MSP_SUCCESS) {
                    Debug.Log("结束失败：（错误码）" + (int) errorCode3);
                    Account.Logout();
                    return;
                }

                // 失败结束退出
                Account.Logout();

                Data = msg.ToString();
                isDown = true;

            } catch (Exception e) {
                Debug.Log(e);
                Account.Logout();
            }
        }

    }
}

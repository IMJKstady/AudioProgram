using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using UnityEngine;


namespace MscAudio {

    /// <summary>
    /// 语音合成
    /// </summary>
    public class SpeechSynthesis {

        /// <summary>
        /// 识别是否完成
        /// 用于在类外部判断合成是否完成
        /// </summary>
        public bool isDown { get; private set; }

        /// <summary>
        /// 识别出的信息
        /// </summary>
        public byte[] Data { get; private set; }
        // 合成的文字
        private string m_Data;
        // 合成控制参数
        private string m_Params = "engine_type = cloud, text_encoding = UTF8, voice_name = xiaoqi";

        /// <summary>
        /// 语音音频头
        /// </summary>
        private struct WAVE_Header {
            public int RIFF_ID;
            public int File_Size;
            public int RIFF_Type;
            public int FMT_ID;
            public int FMT_Size;
            public short FMT_Tag;
            public ushort FMT_Channel;
            public int FMT_SamplesPerSec;
            public int AvgBytesPerSec;
            public ushort BlockAlign;
            public ushort BitsPerSample;
            public int DATA_ID;
            public int DATA_Size;
        }


        public SpeechSynthesis(string data) {

            m_Data = data;
        }

        /// <summary>
        /// 音频写入文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public bool WriteToFile(string path) {

            if (path.Equals("") || path == null) return false;

            if (isDown == true) {

                FileStream fileStream = null;
                if (File.Exists(path)) {
                    fileStream = File.Open(path, FileMode.Open, FileAccess.Write);
                } else {
                    fileStream = File.Create(path);
                }

                fileStream.Flush();
                fileStream.BeginWrite(Data, 0, Data.Length, (obj) => {

                    fileStream.Close();
                    fileStream.Dispose();
                    Debug.Log("写入完成");
                }, null);

            } else {
                Debug.Log("识别还未完成");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 开始合成
        /// </summary>
        public void Begin() {

            // 登录
            if (!Account.Login()) {

                return;
            }

            new Thread(ThreadStart).Start();
        }

        /// <summary>
        /// 合成线程
        /// </summary>
        private void ThreadStart() {

            try {
                ErrorCode errorCode;
                IntPtr ptr = Msc.QTTSSessionBegin(m_Params, out errorCode);

                if (errorCode != ErrorCode.MSP_SUCCESS) {

                    Debug.Log("开始合成出错：（错误码）" + (int) errorCode);
                    Account.Logout();

                    return;
                }

                ErrorCode errorCode1 = Msc.QTTSTextPut(ptr, m_Data, (uint) Encoding.Default.GetByteCount(m_Data), string.Empty);

                if (errorCode1 != ErrorCode.MSP_SUCCESS) {

                    Debug.Log("文本写入出错：（错误码）" + (int) errorCode);
                    Account.Logout();

                    return;
                }

                MemoryStream memoryStream = new MemoryStream();
                SynthStatus synthStatus;
                ErrorCode errorCode2;
                uint length;

                while (true) {

                    IntPtr data = Msc.QTTSAudioGet(ptr, out length, out synthStatus, out errorCode2);

                    if (data != null) {
                        if (length > 0) {
                            byte[] vs = new byte[(int) length];

                            Marshal.Copy(data, vs, 0, (int) length);

                            memoryStream.Write(vs, 0, vs.Length);
                            Thread.Sleep(1000);
                        }
                    }
                    if (errorCode2 != ErrorCode.MSP_SUCCESS) {
                        Debug.Log("获取音频出错：（错误码）" + (int) errorCode2);
                        Account.Logout();

                        return;
                    }

                    if (synthStatus == SynthStatus.MSP_TTS_FLAG_DATA_END)
                        break;
                }


                ErrorCode errorCode3 = Msc.QTTSSessionEnd(ptr, "合成结束");

                WAVE_Header wave_Header = getWave_Header((int) memoryStream.Length);
                byte[] head = StructToBytes(wave_Header);

                memoryStream.Position = 0L;
                memoryStream.Write(head, 0, head.Length);
                memoryStream.Position = 0L;

                if (errorCode3 != ErrorCode.MSP_SUCCESS) {
                    Debug.Log("结束合成音频出错：（错误码）" + (int) errorCode2);
                    Account.Logout();

                    return;
                }

                Account.Logout();

                isDown = true;
                Data = memoryStream.GetBuffer();

                Debug.Log("合成完成");

            } catch (Exception e) {
                Debug.Log(e);
                Account.Logout();
            }
        }


        /// <summary>
        /// 结构体初始化赋值
        /// </summary>
        /// <param name="data_len"></param>
        /// <returns></returns>
        private WAVE_Header getWave_Header(int data_len) {
            return new WAVE_Header {
                RIFF_ID = 1179011410,
                File_Size = data_len + 36,
                RIFF_Type = 1163280727,
                FMT_ID = 544501094,
                FMT_Size = 16,
                FMT_Tag = 1,
                FMT_Channel = 1,
                FMT_SamplesPerSec = 16000,
                AvgBytesPerSec = 32000,
                BlockAlign = 2,
                BitsPerSample = 16,
                DATA_ID = 1635017060,
                DATA_Size = data_len
            };


        }


        /// <summary>
        /// 结构体转字符串
        /// </summary>
        /// <param name="structure"></param>
        /// <returns></returns>
        private byte[] StructToBytes(object structure) {
            int num = Marshal.SizeOf(structure);
            IntPtr intPtr = Marshal.AllocHGlobal(num);
            byte[] result;
            try {
                Marshal.StructureToPtr(structure, intPtr, false);
                byte[] array = new byte[num];
                Marshal.Copy(intPtr, array, 0, num);
                result = array;
            } finally {
                Marshal.FreeHGlobal(intPtr);
            }
            return result;

        }
    }
}

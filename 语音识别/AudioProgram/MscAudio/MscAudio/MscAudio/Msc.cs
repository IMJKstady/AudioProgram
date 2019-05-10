using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;


namespace MscAudio {

    internal class Msc {

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="usr">用户名称</param>
        /// <param name="pwd">密码</param>
        /// <param name="paramter">传入参数</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ErrorCode MSPLogin(string usr, string pwd, string paramter);

        /// <summary>
        /// 用户退出
        /// </summary>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ErrorCode MSPLogout();



        # region 语音识别
        /// <summary>
        /// 开始一次语音识别
        /// </summary>
        /// <param name="paramsList">一些控制参数</param>
        /// <param name="userMpdelld">此参数保留，传入NULL即可</param>
        /// <param name="errorCode">函数调用成功则其值为0，否则返回错误代码</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QISRSessionBegin(string @params, string userModelld, out ErrorCode errorCode);


        /// <summary>
        /// 写入本次识别的音频
        /// </summary>
        /// <param name="sessionID">由QISESessionBegin返回的句柄</param>
        /// <param name="waveData">音频数据</param>
        /// <param name="waveLen">音频数据长度</param>
        /// <param name="audioStatus">用来告知MSC音频发送是否完成</param>
        /// <param name="epStatus">端点检测（End-point detected）器所处的状态</param>
        /// <param name="Status">评测器返回的状态，提醒用户及时开始\停止获取评测结果</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ErrorCode QISRAudioWrite(IntPtr sessionID, byte[] waveData, uint waveLen, AudioStatus audioStatus, out EpStatus epStatus, out RsltStatus recogStatus);


        /// <summary>
        /// 获取识别结果
        /// </summary>
        /// <param name="sessionID">由QISRSessionBegin返回的句柄</param>
        /// <param name="rsltStatus">	识别结果的状态</param>
        /// <param name="waitTime">此参数做保留用</param>
        /// <param name="errorCode">函数调用成功则其值为0，否则返回错误代码</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QISRGetResult(IntPtr sessionID, out RsltStatus rsltStatus, int waitTime, out ErrorCode errorCode);

        /// <summary>
        /// 结束本次语音识别
        /// </summary>
        /// <param name="sessionID">由QISESessionBegin返回的句柄</param>
        /// <param name="hints">结束本次语音评测的原因描述，为用户自定义内容</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ErrorCode QISRSessionEnd(IntPtr sessionID, string hints);
        #endregion


        #region 语音写入
        /// <summary>
        /// 开始一次语音合成，分配语音合成资源。
        /// </summary>
        /// <param name="params">传入的参数列表</param>
        /// <param name="errorCode">函数调用成功则其值为0，否则返回错误代码</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QTTSSessionBegin(string @params, out int errorCode);


        /// <summary>
        /// 写入要合成的文本。
        /// </summary>
        /// <param name="sessionID">由QTTSSessionBegin返回的句柄</param>
        /// <param name="textString">字符串指针。指向待合成的文本字符串</param>
        /// <param name="textLen">合成文本长度,最大支持8192个字节（不含’\0’）</param>
        /// <param name="params">本次合成所用的参数，只对本次合成的文本有效。目前为空</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ErrorCode QTTSTextPut(IntPtr sessionID, string textString, int textLen, string @params);

        /// <summary>
        /// 获取合成音频
        /// @
        /// @用户需要反复获取音频，直到音频获取完毕或函数调用失败。在重复获取音频时，如果暂未获得音频数据，需要将当前线程sleep一段时间，以防频繁调用浪费CPU资源。
        /// </summary>
        /// <param name="sessionID">由QTTSSessionBegin返回的句柄</param>
        /// <param name="audioLen">合成音频长度，单位字节</param>
        /// <param name="synthStatus">合成音频状态</param>
        /// <param name="errorCode">函数调用成功则其值为M0，否则返回错误代码</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr QTTSAudioGet(IntPtr sessionID, int audioLen, out SynthStatus synthStatus, out int errorCode);

        /// <summary>
        /// 结束本次语音合成。
        /// </summary>
        /// <param name="sessionID">由QTTSSessionBegin返回的句柄</param>
        /// <param name="hints">结束本次语音合成的原因描述，为用户自定义内容</param>
        /// <returns></returns>
        [DllImport("msc.dll", CallingConvention = CallingConvention.StdCall)]
        public static extern ErrorCode QTTSSessionEnd(IntPtr sessionID, string hints);
        #endregion
    }

}

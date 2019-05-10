

namespace MscAudio {


    /// <summary>
    /// 用来告知MSC音频发送是否完成
    /// </summary>
    enum AudioStatus {
        /// <summary>
        /// 第一块音频
        /// </summary>
        MSP_AUDIO_SAMPLE_FIRST = 1,
        /// <summary>
        /// 还有后继音频
        /// </summary>
        MSP_AUDIO_SAMPLE_CONTINUE = 2,

        /// <summary>
        /// 最后一块音频
        /// </summary>
        MSP_AUDIO_SAMPLE_LAST = 4

    }


    /// <summary>
    /// 识别器返回的状态，提醒用户及时开始\停止获取识别结果
    /// </summary>
    enum RsltStatus {
        /// <summary>
        /// 识别成功，此时用户可以调用QISRGetResult来获取（部分）结果。
        /// </summary>
        MSP_REC_STATUS_SUCCESS = 0,
        /// <summary>
        /// 识别结束，没有识别结果。
        /// </summary>
        MSP_REC_STATUS_NO_MATCH = 1,
        /// <summary>
        /// 正在识别中。
        /// </summary>
        MSP_REC_STATUS_INCOMPLETE = 2,
        /// <summary>
        /// 识别结束。
        /// </summary>
        MSP_REC_STATUS_COMPLETE = 5,
    }

    /// <summary>
    /// 端点检测（End-point detected）器所处的状态
    /// </summary>
    enum EpStatus {
        /// <summary>
        /// 还没有检测到音频的前端点。
        /// </summary>
        MSP_EP_LOOKING_FOR_SPEECH = 0,
        /// <summary>
        /// 已经检测到了音频前端点，正在进行正常的音频处理。
        /// </summary>
        MSP_EP_IN_SPEECH = 1,
        /// <summary>
        /// 检测到音频的后端点，后继的音频会被MSC忽略。
        /// </summary>
        MSP_EP_AFTER_SPEECH = 3,
        /// <summary>
        /// 超时。
        /// </summary>
        MSP_EP_TIMEOUT = 4,
        /// <summary>
        /// 出现错误。
        /// </summary>
        MSP_EP_ERROR = 5,
        /// <summary>
        /// 音频过大。
        /// </summary>
        MSP_EP_MAX_SPEECH = 6,
    }

    /// <summary>
    /// 合成音频状态
    /// </summary>
    enum SynthStatus {
        /// <summary>
        /// 音频还没取完，还有后继的音频
        /// </summary>
        MSP_TTS_FLAG_STILL_HAVE_DATA = 1,
        /// <summary>
        /// 音频已经取完
        /// </summary>
        MSP_TTS_FLAG_DATA_END = 2,
    }
}

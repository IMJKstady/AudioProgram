/*
* AIUIAgentTest.h
*
*  Created on: 2017年3月9日
*      Author: hj
*/

#ifndef AIUIAGENTTEST_H_
#define AIUIAGENTTEST_H_

#include <aiui/AIUI.h>


#include <string>
#include <iostream>

#include "FileUtil.h"

#ifdef _MSC_VER
#include <windows.h>
#include <process.h>
#define TEST_ROOT_DIR ".\\AIUI\\"
#define CFG_FILE_PATH TEST_ROOT_DIR##"cfg\\aiui.cfg"
#define TEST_AUDIO_PATH TEST_ROOT_DIR##"audio\\test.pcm"
#define LOG_DIR TEST_ROOT_DIR##"log"
#define SYNC_PARAM_PATH TEST_ROOT_DIR##"test\\syncstat.json"
#else
#include <unistd.h>
#define TEST_ROOT_DIR "./AIUI"
#define CFG_FILE_PATH TEST_ROOT_DIR##"/cfg/aiui.cfg"
#define TEST_AUDIO_PATH TEST_ROOT_DIR##"/test/audio/test.pcm"
#define LOG_DIR TEST_ROOT_DIR##"/log"
#endif

using namespace aiui;
using namespace std;


class WriteAudioThread 
{
private:
	IAIUIAgent* mAgent;

	string mAudioPath;

	bool mRepeat;

	bool mRun;

	FileUtil::DataFileHelper* mFileHelper;

	HANDLE  thread_hdl_;
	unsigned int  thread_id_;


private:
	bool threadLoop();

	static unsigned int __stdcall WriteProc(void * paramptr) ;


public:
	WriteAudioThread(IAIUIAgent* agent, const string& audioPath, bool repeat);

	~WriteAudioThread();

	void stopRun();

	bool run();

};


class TestListener : public IAIUIListener
{
private:
	FileUtil::DataFileHelper* mTtsFileHelper;

public:
	TestListener();

	~TestListener();

	void onEvent(const IAIUIEvent& event) const;
};


class AIUITester
{
private:
	IAIUIAgent* agent;

	/* AIUI事件回调监听器 */
	TestListener listener;

	WriteAudioThread * writeThread;

	string encode(const unsigned char* bytes_to_encode, unsigned int in_len);

public:
	AIUITester() ;

	~AIUITester();
private:

	void showIntroduction(bool detail);

	/*创建AIUI代理*/
	void createAgent();

	/* 唤醒接口 */
	void wakeup();

	void start();

	void stop();

	void reset();

	void destory();

	void stopWriteThread();

	/* 语音语义接口 */
	void write(bool repeat);

	/* 文本语义接口 */
	void writeText();

	/* 动态上传资源数据 */
	void syncSchema();

	void querySyncStatus();

	/* 开始tts */
	void startTts();

	/* 暂停tts */
	void pauseTts();

	/* 继续上次的tts */
	void resumeTts();

	/* 取消本次tts */
	void cancelTts();

public:
	void readCmd();
	void test();
};

#endif /* AIUIAGENTTEST_H_ */

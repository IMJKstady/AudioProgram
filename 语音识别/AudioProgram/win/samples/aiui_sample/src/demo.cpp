
#include <iostream>

#include "AIUITest.h"

using namespace std;
using namespace aiui;

/*
 *  c       创建
 *  w       唤醒SDK
 *  wr      写音频
 *  wrt     写文本
 *  swrt    停止写线程
 *  d       销毁
 *  q       退出
 *  
 */

/* 结果编码为 UTF-8， 测试使用的终端显示编码使用UTF-8 */

int main()
{
	AIUITester t;
	t.test();
	return 0;
}

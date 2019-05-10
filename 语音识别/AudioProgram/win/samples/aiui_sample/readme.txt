更新说明：

如果之前有在开放平台上下载我们的旧版本SDK，需要将该新版本集成到旧版本之中，除了将aiui.dll和aiui.lib替换之外，还要注意以下改动：

1、新版本头文件有改动，具体改动请自行对比下，主要是IAIUIListener类中的onEvent()方法的定义改动；

2、AIUITest.cpp中createAgent()方法有变动，建议对比下新旧版本之间AIUITest.cpp的不同之处。

3、1026版本之后，aiui_sample支持ivw单路唤醒功能，如果要使用ivw单路唤醒，首先需要自行下载到支持唤醒的msc.dll和对应的唤醒资源，msc.dll放在bin目录下即可，demo目前支持50、60唤醒，
另外需在aiui.cfg中做如下两项配置：
	1)、speech->wakeup_mode为“ivw”, 默认情况下该值为“off”;
	2)、ivw->res_path设置为唤醒资源的路径
	wakeup_mode设置为"ivw"时，就不需要手动输入w命令发送wake_up消息进行外部唤醒了
	
	ps: 需要说使用ivw单路唤醒，需要自行调用MSCLogin()接口，参考代码中注释部分；
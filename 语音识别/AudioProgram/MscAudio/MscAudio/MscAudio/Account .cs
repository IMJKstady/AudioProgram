using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace MscAudio {


    public class Account {

        /// <summary>
        /// 是否登录
        /// </summary>
        public static bool Islogged { get; private set; }
        /// <summary>
        /// 账户名
        /// </summary>
        public static string User = "18274283128";
        /// <summary>
        /// 密码
        /// </summary>
        public static string Pwd = "hzlhzl520";
        /// <summary>
        /// 传入控制参数
        /// </summary>
        public static string Paramter = "appid = 5cc17240";

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="usr">用户名称</param>
        /// <param name="pwd">密码</param>
        /// <param name="paramter">传入参数</param>
        public static bool Login() {

            if (User.Equals("") || Pwd.Equals("") || Paramter.Equals("")) {
                Console.WriteLine("账户数据存在空的");
            }
            ErrorCode error = Msc.MSPLogin(User, Pwd, Paramter);

            if (error != ErrorCode.MSP_SUCCESS) {
                Console.WriteLine("账号登录失败：（错误码）" + (int)error);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 退出
        /// </summary>
        public static bool Logout() {

            ErrorCode errorCode = Msc.MSPLogout();

            if (errorCode != ErrorCode.MSP_SUCCESS) {
                Console.WriteLine("账号登录失败：（错误码）" + (int)errorCode);
                return false;
            }
            return true;
        }
    }
}

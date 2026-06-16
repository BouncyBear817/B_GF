// /************************************************************
//  * Unity Version: 2022.3.15f1c1
//  * Author:        bear
//  * CreateTime:    2024/9/3 16:31:54
//  * Description:
//  * Modify Record:
//  *************************************************************/

using GameFramework;

namespace GameMain
{
    public class BLogHelper : GameFrameworkLog.ILogHelper
    {
        public void Log(GameFrameworkLogLevel level, object message)
        {
            switch (level)
            {
                case GameFrameworkLogLevel.Debug:
                    BLogger.Debug(message.ToString());
                    break;
                case GameFrameworkLogLevel.Info:
                    BLogger.Info(message.ToString());
                    break;
                case GameFrameworkLogLevel.Warning:
                    BLogger.Warning(message.ToString());
                    break;
                case GameFrameworkLogLevel.Error:
                    BLogger.Error(message.ToString());
                    break;
                case GameFrameworkLogLevel.Fatal:
                    BLogger.Fatal(message.ToString());
                    break;
                default:
                    throw new GameFrameworkException(message.ToString());
            }
        }
    }
}
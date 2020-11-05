using Grpc.Core;
using AL.Common.Data.Enums;
using AL.Common.Tools;
using MagicOnion.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AL.Common.Base.Base
{

    /// <summary>
    /// /grpc客户端
    /// </summary>
    public static class GrpcClientFactory
    {
        public static Channel GetClient(GrpcClients client)
        {
            //获取配置文件
            string Name = Enum.GetName(typeof(GrpcClients), client);
            string host = LocalAppsetting.GetSettingNode(new string[] { "Startup", "Grpc", Name, "IP" });
            int port = LocalAppsetting.GetSettingNode(new string[] { "Startup", "Grpc", Name, "Port" }).ToInt();
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(host))
                throw new Exception("Grpc客户端名称未配置！");
            var channel = new Channel(host, port, ChannelCredentials.Insecure);
            channel.ConnectAsync().Wait();
            if (channel.State == ChannelState.Connecting || channel.State == ChannelState.TransientFailure)
                throw new Exception("Grpc客户端服务处于停止状态");
            return channel;
        }


    }
}

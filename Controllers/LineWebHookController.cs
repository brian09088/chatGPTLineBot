using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace isRock.Template
{
    public class LineWebHookController : isRock.LineBot.LineWebHookControllerBase
    {
        [Route("api/LineBotWebHook")]
        [HttpPost]
        public IActionResult POST()
        {
            var AdminUserId = "U706469d6fe736b758642171f0c9ebefd";

            try
            {
                //設定ChannelAccessToken
                this.ChannelAccessToken = "9XL55l85hlXA9CR9vLsH9T6Nk3DoWkE6AyDlcNFbkrH86s6JPRBDYbWbRDdoRO5gQoPldQglMdwHlHooLyNyC1lyf6UCP89CrjN9A+MY97sSmy70XXsuGkPyMOSwSkihL7X/jSzVpakdBSmc+r5azgdB04t89/1O/w1cDnyilFU=";
                //配合Line Verify
                if (ReceivedMessage.events == null || ReceivedMessage.events.Count() <= 0 ||
                    ReceivedMessage.events.FirstOrDefault().replyToken == "00000000000000000000000000000000") return Ok();
                //取得Line Event
                var LineEvent = this.ReceivedMessage.events.FirstOrDefault();
                var responseMsg = "";
                //準備回覆訊息
                if (LineEvent.type.ToLower() == "message" && LineEvent.message.type == "text")
                {
                    var GptResult=isRock.Template.ChatGPT.CallChatGPT(LineEvent.message.text).choices[0].message.content;
                    responseMsg = $"{GptResult}";
                }
                else if (LineEvent.type.ToLower() == "message")
                    responseMsg = $"收到 event : {LineEvent.type} type: {LineEvent.message.type} ";
                else
                    responseMsg = $"收到 event : {LineEvent.type} ";
                //回覆訊息
                this.ReplyMessage(LineEvent.replyToken, responseMsg);
                //response OK
                return Ok();
            }
            catch (Exception ex)
            {
                //回覆訊息
                this.PushMessage(AdminUserId, "發生錯誤:\n" + ex.Message);
                //response OK
                return Ok();
            }
        }
    }
}

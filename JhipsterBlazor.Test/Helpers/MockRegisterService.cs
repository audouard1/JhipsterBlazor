﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using JhipsterBlazor.Models;
using JhipsterBlazor.Services.AccountServices;
using SharedModel.Constants;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JhipsterBlazor.Test.Helpers
{
    public class MockRegisterService : IRegisterService
    {
        public virtual Task<HttpResponseMessage> Save(UserSaveModel registerModel)
        {
            var resultMsg = new RegisterResultRequest();
            resultMsg.Detail = "";
            resultMsg.Params = "";
            resultMsg.Status = 400;
            resultMsg.Params = "";
            resultMsg.TraceId = "";
            resultMsg.Type = "";
            if (registerModel.Login == "testSuccess")
            {
                return Task.Run(() => new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent("")
                });
            }

            if (registerModel.Login == "testEmail")
            {
                resultMsg.Type = ErrorConst.EmailAlreadyUsedType;
            }

            if (registerModel.Login == "testLogin")
            {
                resultMsg.Type = ErrorConst.LoginAlreadyUsedType;
            }

            return Task.Run(() => new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = new StringContent(JsonSerializer.Serialize(resultMsg), Encoding.UTF8, "application/json")
            });

        }
    }
}

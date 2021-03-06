﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Weixin.Core.Helper
{
    public class CommonHelper
    {
        public static string CalcMD5(string str)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
            return CalcMD5(bytes);
        }

        public static string CalcMD5(byte[] bytes)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(bytes);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }

        public static string CalcMD5(Stream stream)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] computeBytes = md5.ComputeHash(stream);
                string result = "";
                for (int i = 0; i < computeBytes.Length; i++)
                {
                    result += computeBytes[i].ToString("X").Length == 1 ? "0" + computeBytes[i].ToString("X") : computeBytes[i].ToString("X");
                }
                return result;
            }
        }

        //Chapcha
        public static string CreateVerifyCode(int len)
        {
            char[] data = { 'a','c','d','e','f','h','k','m',
                'n','r','s','t','w','x','y'};
            StringBuilder sb = new StringBuilder();
            Random rand = new Random();
            for (int i = 0; i < len; i++)
            {
                int index = rand.Next(data.Length);//[0,data.length)
                char ch = data[index];
                sb.Append(ch);
            }
            //勤测！
            return sb.ToString();
        }

        public static string CreatePageBar(int pageIndex, int pageCount, string controller, 
            string action, params string[] parameters)
        {
            if (pageCount == 1) //如果只有一页，就不用显示分页页码了
            {
                return string.Empty;
            }
            int start = pageIndex - 4;
            start = start < 1 ? 1 : start;
            int end = pageIndex + 4;
            end = end > pageCount ? pageCount : end;
            StringBuilder sb = new StringBuilder();
            sb.Append("<ul class=\"pager\">");
            if (pageIndex != 1)
            {
                sb.Append($"<li class=\"previous\"><a href='/{controller}/{action}?@paramsPlaceholder'>首页</a></li>");
            }
            for (int i = start; i <= end; i++)
            {
                if (i == pageIndex) //如果是当前页，则不需要添加超链接
                {
                    sb.Append($"<li class=\"active\"><a href=\"#\">{i}</a></li>");
                }
                else
                {
                    sb.Append($"<li class=\"previous\"><a href='/{controller}/{action}?pageIndex={i}@paramsPlaceholder'>{i}</a></li>");
                }
            }
            if (pageIndex != pageCount)
            {
                sb.Append($"<li class=\"next\"><a href='/{controller}/{action}?pageIndex={pageCount}@paramsPlaceholder'>末页</a></li>");
            }

            string paramsPlaceholder = string.Empty;

            if (parameters != null)
            {
                foreach (var item in parameters) //遍历参数
                {
                    if (!string.IsNullOrEmpty(item)) //如果有参数，则添加进占位符
                    {
                        paramsPlaceholder += "&" + item;
                    }
                }
            }

            sb.Append("</ul>");
            return sb.ToString().Replace("@paramsPlaceholder", paramsPlaceholder); //将占位符替换为参数
        }
    }
}

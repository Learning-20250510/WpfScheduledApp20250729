using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimplyTaskSystem.Models.Utility
{
    class ValidateString
    {
        /// <summary>
        /// 指定された文字列が URL(https:~~) かどうかを返します
        /// </summary>
        public  bool IsUrl_1(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(
               input,
               @"^s?https?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"
            );
        }


        /// <summary>
        /// 指定された文字列が URL(http:~~) かどうかを返します
        /// </summary>
        public  bool IsUrl_2(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }
            return Regex.IsMatch(
               input,
               @"^s?http?://[-_.!~*'()a-zA-Z0-9;/?:@&=+$,%#]+$"
            );
        }
    }
}

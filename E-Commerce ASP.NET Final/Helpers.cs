using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace E_Commerce_ASP.NET_Final
{
    public class Helpers
    {
        public static int[][] StringToCartArray(string cart)
        {
            string[] arr = cart.Split("/".ToCharArray()[0]);

            int[][] cartArray = new int[arr.Length][];

            for (int i = 0; i < arr.Length; i++)
            {
                string[] infos = arr[i].Split("-".ToCharArray()[0]);

                cartArray[i] = new int[2];
                cartArray[i][0] = Convert.ToInt32(infos[0]);
                cartArray[i][1] = Convert.ToInt32(infos[1]);
            }

            return cartArray;
        }

        public static string CartArrayToString(int[][] cartArray)
        {
            string rtn = "";
            for (int i = 0; i < cartArray.Length; i++)
            {
                rtn += cartArray[i][0] + "-" + cartArray[i][1];

                if (i != cartArray.Length - 1) rtn += "/";
            }

            return rtn;
        }
    }
}
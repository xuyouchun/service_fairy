using System;
using System.Collections.Generic;
using System.Text;

using MSXML2;

namespace BohaoCheckinDeamon
{
  public class Conn
  {
    private ServerXMLHTTP30Class _xmlhttp;

    public Conn()
    {
      this._xmlhttp = new ServerXMLHTTP30Class();
    }

    public string PostData(string url, Dictionary<string,string> postData)
    {
      string retData = String.Empty;

      List<string> myPostData = new List<string>();
      foreach (var n in postData)
      {
        myPostData.Add(n.Key + "=" + n.Value);
      }

      try
      {
        _xmlhttp.open("PUT", url, false, null, null);
        _xmlhttp.setRequestHeader("Content-Type", "application/x-www-form-urlencoded");
        _xmlhttp.setRequestHeader("Connection", "Keep-Alive");

        _xmlhttp.send(String.Join("&", myPostData.ToArray()));

        if (_xmlhttp.readyState == 4)
        {
          retData = System.Text.Encoding.UTF8.GetString((byte[])_xmlhttp.responseBody);
        }

        return retData;
      }
      catch
      {
        return String.Empty;
      }
    }
  }
}

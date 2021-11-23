using System;
using System.Collections.Generic;
using System.Text;

namespace FindMyPWD.Model
{
    public class BLEDevice
    {
        public string _name { get; set;  }
        public string _id { get; set; }

        public BLEDevice(string name, string id)
        {
            _name = name;
            _id = id;
        }
    }
}

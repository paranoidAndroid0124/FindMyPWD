using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace FindMyPLWD
{

    public class DeviceData
    {
        private String name;
        private String address;
        private String additionalInfo;

        //private int cellNum; //set when saving device info
        //private int bluetoothNum; //set when bluetooth save


        public DeviceData()
        {
            this.name = null;
            this.address = null;
            this.additionalInfo = null;
        }

        public DeviceData(String name, String address, String additionalInfo)
        {
            this.name = name;
            this.address = address;
            this.additionalInfo = additionalInfo;
        }

        public String getName()
        {
            return this.name;
        }

        public String getAddress()
        {
            return this.address;
        }

        public String getAdditionalInfo()
        {
            return this.additionalInfo;
        }

        /*
         * public int getCellNum()
         * {
         * return this.cellNum
         * }
         * 
         * public int getBluetoothNum()
         * {
         * return this.bluetoothNum;
         * }
         */


        public void setName(String name)
        {
            this.name = name;
        }

        public void setAddress(String address)
        {
            this.address = address;
        }

        public void setAdditionalInfo(String additionalInfo)
        {
            this.additionalInfo = additionalInfo;
        }


        /*
         * public void setCellNum(int cellNum)
         * {
         * this.cellNum = cellNum;
         * }
         * 
         * public void setBluetoothNum(int bluetoothNum)
         * {
         * this.bluetoothNum = bluetoothNum;
         * }
         */
    }
}

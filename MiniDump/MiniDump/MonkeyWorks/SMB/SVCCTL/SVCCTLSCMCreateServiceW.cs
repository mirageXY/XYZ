﻿using System;
using System.Linq;
using System.Text;

namespace MonkeyWorks.SMB.SVCCTL
{
    sealed class SVCCTLSCMCreateServiceW
    {
        private Byte[] ContextHandle;
        private Byte[] ServiceName_MaxCount;
        private Byte[] ServiceName_Offset = { 0x00, 0x00, 0x00, 0x00 };
        private Byte[] ServiceName_ActualCount;
        private Byte[] ServiceName;
        private Byte[] DisplayName_ReferentID;
        private Byte[] DisplayName_MaxCount;
        private readonly Byte[] DisplayName_Offset = { 0x00, 0x00, 0x00, 0x00 };
        private Byte[] DisplayName_ActualCount;
        private Byte[] DisplayName;
        private readonly Byte[] AccessMask = { 0xff, 0x01, 0x0f, 0x00 };
        private readonly Byte[] ServiceType = { 0x10, 0x00, 0x00, 0x00 };
        private readonly Byte[] ServiceStartType = { 0x03, 0x00, 0x00, 0x00 };
        private readonly Byte[] ServiceErrorControl = { 0x00, 0x00, 0x00, 0x00 };
        private Byte[] BinaryPathName_MaxCount;
        private readonly Byte[] BinaryPathName_Offset = { 0x00, 0x00, 0x00, 0x00 };
        private Byte[] BinaryPathName_ActualCount;
        private Byte[] BinaryPathName;
        private readonly Byte[] LoadOrderGroup = { 0x00, 0x00, 0x00, 0x00 };
        private readonly Byte[] TagID = { 0x00, 0x00, 0x00, 0x00 };
        private readonly Byte[] Dependencies = { 0x00, 0x00, 0x00, 0x00 };
        private readonly Byte[] DependSize = { 0x00, 0x00, 0x00, 0x00 };
        private readonly Byte[] ServiceStartName = { 0x00, 0x00, 0x00, 0x00 };
        private readonly Byte[] Password = { 0x00, 0x00, 0x00, 0x00 };
        private readonly Byte[] PasswordSize = { 0x00, 0x00, 0x00, 0x00 };

        private String strServiceName;

        internal SVCCTLSCMCreateServiceW()
        {
            DisplayName_ReferentID = Combine.combine(BitConverter.GetBytes(GenerateUuidNumeric(2)).Take(2).ToArray(), new Byte[] { 0x00, 0x00 });
        }
        
        internal void SetContextHandle(Byte[] ContextHandle)
        {
            this.ContextHandle = ContextHandle;
        }

        internal void SetServiceName()
        {
            strServiceName = GenerateUuidAlpha(20);
            Byte[] tmp = Combine.combine(Encoding.Unicode.GetBytes(strServiceName), new Byte[] { 0x00, 0x00, 0x00, 0x00 });
            ServiceName = DisplayName = tmp;
            ServiceName_MaxCount = ServiceName_ActualCount = DisplayName_MaxCount = DisplayName_ActualCount = BitConverter.GetBytes(strServiceName.Length + 1);
        }

        internal String GetServiceName()
        {
            return strServiceName;
        }

        internal void SetServiceName(String strServiceName)
        {
            this.strServiceName = strServiceName;
            Byte[] tmp = Combine.combine(Encoding.Unicode.GetBytes(strServiceName), new Byte[] { 0x00, 0x00 });
            if (0 != strServiceName.Length % 2)
            {
                tmp = Combine.combine(tmp, new Byte[] { 0x00, 0x00 });
            }
            ServiceName = DisplayName = tmp;
            ServiceName_MaxCount = ServiceName_ActualCount = DisplayName_MaxCount = DisplayName_ActualCount = BitConverter.GetBytes(strServiceName.Length + 1);
        }

        internal void SetCommand(String command)
        {
            BinaryPathName = Combine.combine(Encoding.Unicode.GetBytes(command), new Byte[] { 0x00, 0x00 });
            if (0 != command.Length % 2)
            {
                //BinaryPathName = Combine.combine(BinaryPathName, new Byte[] { 0x00, 0x00 });
            }
            BinaryPathName_MaxCount = BinaryPathName_ActualCount = BitConverter.GetBytes(command.Length + 1);
        }

        internal Byte[] GetRequest()
        {
            Combine combine = new Combine();
            combine.Extend(ContextHandle);
            combine.Extend(ServiceName_MaxCount);
            combine.Extend(ServiceName_Offset);
            combine.Extend(ServiceName_ActualCount);
            combine.Extend(ServiceName);
            combine.Extend(DisplayName_ReferentID);
            combine.Extend(DisplayName_MaxCount);
            combine.Extend(DisplayName_Offset);
            combine.Extend(DisplayName_ActualCount);
            combine.Extend(DisplayName);
            combine.Extend(AccessMask);
            combine.Extend(ServiceType);
            combine.Extend(ServiceStartType);
            combine.Extend(ServiceErrorControl);
            combine.Extend(BinaryPathName_MaxCount);
            combine.Extend(BinaryPathName_Offset);
            combine.Extend(BinaryPathName_ActualCount);
            combine.Extend(BinaryPathName);
            combine.Extend(LoadOrderGroup);
            combine.Extend(TagID);
            combine.Extend(Dependencies);
            combine.Extend(DependSize);
            combine.Extend(ServiceStartName);
            combine.Extend(Password);
            combine.Extend(PasswordSize);
            return combine.Retrieve();
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        ////////////////////////////////////////////////////////////////////////////////
        internal static String GenerateUuidAlpha(int length)
        {
            Random random = new Random();
            const String chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            return new String(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        ////////////////////////////////////////////////////////////////////////////////
        //
        ////////////////////////////////////////////////////////////////////////////////
        internal static Int32 GenerateUuidNumeric(int length)
        {
            Random random = new Random();
            const String chars = "0123456789";
            String strUUID = new String(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
            Int32 uuid = 0;
            if (Int32.TryParse(strUUID, out uuid))
            {
                return uuid;
            }
            return 0;
        }
    }
}

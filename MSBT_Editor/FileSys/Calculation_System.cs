﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.Windows.Forms;

using EN = System.Environment;
using MSBT_Editor.Sectionsys;
using MSBT_Editor.Formsys;

namespace MSBT_Editor.FileSys
{
    class Calculation_System
    {
        public static string Byte2Char(BinaryReader br, int readchers = 4)
        {
            return new string(br.ReadChars(readchers));
        }

        public static string Byte2Str_UTF16BE(BinaryReader br, int readchers = 2)
        {
            var strs = Encoding.GetEncoding("unicodeFFFE").GetString(br.ReadBytes(readchers));
            return strs;
        }

        public static void UTF16BE(string str, List<byte> bits) {
            var ret = Encoding.GetEncoding("unicodeFFFE").GetBytes(str);
            foreach (byte bit in ret) bits.Add(bit);
        }

        public static string Byte2JIS(BinaryReader br, FileStream fs)
        {
            List<byte> bit_list = new List<byte>();
            byte bit1, bit2;
            byte bit3, bit4, bit5, bit6, bit7, bit8;
            bool checker = true;
            while (checker) {
                bit1 = br.ReadByte();
                bit2 = br.ReadByte();
                bit_list.Add(bit1);
                bit_list.Add(bit2);
                if (bit1 == 0x00 && bit2 == 0x0A) {
                    UTF16BE("</br>" + EN.NewLine, bit_list);
                    continue;
                }



                if (((bit_list[bit_list.Count - 2] == 0x00) && (bit_list[bit_list.Count - 1]) == 0x0E)) {
                    bit_list.RemoveRange(bit_list.Count() - 1, 1);
                    bit_list.RemoveRange(bit_list.Count() - 1, 1);
                    bit3 = br.ReadByte();
                    bit4 = br.ReadByte();
                    bit5 = br.ReadByte();
                    bit6 = br.ReadByte();
                    bit7 = br.ReadByte();
                    bit8 = br.ReadByte();


                    switch (bit4) {
                        case 0x00:
                            Tag00(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            continue;
                        case 0x01:
                            Tag01(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            continue;
                        case 0x03:
                            Tag03(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            continue;
                        case 0x04:
                            Tag04(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            continue;
                        case 0x05:
                            Tag05(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            break;
                        case 0x06:
                            Tag06(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            break;
                        case 0x07:
                            Tag07(br, fs, bit_list, bit5, bit6, bit7, bit8);
                            break;
                        default:
                            break;
                    }

                    continue;
                }

                if ((bit_list[bit_list.Count - 2] == 0) && (bit_list[bit_list.Count - 1]) == 0) {
                    bit_list.RemoveRange(bit_list.Count() - 1, 1);
                    bit_list.RemoveRange(bit_list.Count() - 1, 1);
                    UTF16BE("</End>", bit_list);
                    checker = false;
                }

            }
            //Console.WriteLine(Encoding.GetEncoding("unicodeFFFE"/*1201*/).GetString(bit_list.ToArray()));
            //Debugger.Text("");
            return Encoding.GetEncoding("unicodeFFFE"/*1201*/).GetString(bit_list.ToArray());

        }


        public static bool Tag00(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8)
        {
            var bit9 = Bytes2Byte(br);
            var bit10 = Bytes2Byte(br);
            string tagstrs = "<Unknown=\"Tag00\">";
            switch (bit6)
            {
                case 0x00:
                    var bit11 = Bytes2Byte(br);
                    var bit12 = Bytes2Byte(br);
                    var rubi = (bit12 >> 1);
                    var target = (bit10 >> 1);
                    tagstrs = "<Rubi=\"" + rubi.ToString() + "\" Target=\"" + target.ToString() + "\">";
                    
                    break;
                case 0x03:
                    tagstrs = Tag_Colors(bit10);
                    break;
            }

            UTF16BE(tagstrs, list);
            return true;
        }

        public static string Tag_Colors(byte bit10) {
            var tagname = "";
            switch (bit10)
            {
                case 0x00:
                    tagname = "<Color=\"Black\">";
                    break;
                case 0x01:
                    tagname = "<Color=\"Red\">";
                    break;
                case 0x02:
                    tagname = "<Color=\"Green\">";
                    break;
                case 0x03:
                    tagname = "<Color=\"Blue\">";
                    break;
                case 0x04:
                    tagname = "<Color=\"Yellow\">";
                    break;
                case 0x05:
                    tagname = "<Color=\"Purple\">";
                    break;
                case 0x06:
                    tagname = "<Color=\"Orange\">";
                    break;
                case 0x07:
                    tagname = "<Color=\"Gray\">";
                    break;
                case 0xFF:
                    tagname = "</Color>";
                    break;

            }
            return tagname;
        }


        public static bool Tag01(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8) {

            string tagstrs = "< Unknown =\"Tag01\">";
            var bit9 = Bytes2Byte(br);
            var bit10 = Bytes2Byte(br);
            switch (bit6) {
                case 0x00:

                    if ((bit8 == 2))
                    {
                        var time = (bit9 << 8) + bit10;
                        tagstrs = "</Timer=\"" + time.ToString() + "\">";

                    }
                    break;
                case 0x01:

                    //10byte目が0x0Aではない場合の処理
                    tagstrs = "</NCenter>";

                    //新しいページ作成処理
                    if ((bit8 == 0) && (bit9 == 0x00) && (bit10 == 0x0A)) {
                        tagstrs = "</New_Page>" + EN.NewLine + EN.NewLine;
                        break;
                    }

                    //9,10bitを使用しない場合2bit分fsを戻す
                    fs.Position -= 2;

                    break;
                case 0x02:
                    tagstrs = "</YCenter>";

                    //9,10bitを使用しない場合2bit分fsを戻す
                    fs.Position -= 2;
                    break;
                case 0x03:
                    tagstrs = "</XCenter>";

                    //9,10bitを使用しない場合2bit分fsを戻す
                    fs.Position -= 2;
                    break;
            }

            UTF16BE(tagstrs, list);
            return true;
        }


        public static bool Tag03(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8)
        {
            var bit9 = Bytes2Byte(br);
            var bit10 = Bytes2Byte(br);

            var icon_num = "0003" + bit5.ToString("X2") + bit6.ToString("X2") + bit7.ToString("X2") + bit8.ToString("X2") + bit9.ToString("X2") + bit10.ToString("X2");
            Console.WriteLine(icon_num);

            string tagstrs = "<Unknown=\"Tag03\">" + "<Icon=\"" + icon_num + "\">";
            switch (icon_num)
            {
                case "000300350002003A":
                    tagstrs = "<Icon=\"CometMedal\">";
                    break;
                case "000300360002003B":
                    tagstrs = "<Icon=\"SilverCrown\">";
                    break;
                case "0003002500020025":
                    tagstrs = "<Icon=\"PointerHand\">";
                    break;
                case "0003002800020028":
                    tagstrs = "<Icon=\"Peach\">";
                    break;
                case "0003003F00020044":
                    tagstrs = "<Icon=\"SilverCrownwJewel\">";
                    break;
                case "0003004100020046":
                    tagstrs = "<Icon=\"Begoman\">";
                    break;
                case "0003002300020023":
                    tagstrs = "<Icon=\"Koopa\">";
                    break;
                case "0003004300020048":
                    tagstrs = "<Icon=\"Coins\">";
                    break;
                case "0003002700020027":
                    tagstrs = "<Icon=\"Starbit\">";
                    break;
                case "0003003D00020042":
                    tagstrs = "<Icon=\"BronzeStar\">";
                    break;
                case "0003002D00020032":
                    tagstrs = "<Icon=\"LifeUpMushroom\">";
                    break;
                case "0003001500020015":
                    tagstrs = "<Icon=\"StarPiece\">";
                    break;
                case "0003000A0002000A":
                    tagstrs = "<Icon=\"Pointer\">";
                    break;
                case "0003001F0002001F":
                    tagstrs = "<Icon=\"GreenComet\">";
                    break;
                case "0003000300020003":
                    tagstrs = "<Icon=\"WiiMote\">";
                    break;
                case "000300470002004C":
                    tagstrs = "<Icon=\"Kinopio\">";
                    break;
                case "0003000B0002000B":
                    tagstrs = "<Icon=\"PurpleStarbit\">";
                    break;
                case "0003002900020029":
                    tagstrs = "<Icon=\"Letter\">";
                    break;
                case "0003004200020047":
                    tagstrs = "<Icon=\"Kuribo\">";
                    break;
                case "0003002B0002002B":
                    tagstrs = "<Icon=\"Mario\">";
                    break;
                case "0003001200020012":
                    tagstrs = "<Icon=\"Mario2\">";
                    break;
                case "0003001300020013":
                    tagstrs = "<Icon=\"DPad\">";
                    break;
                case "000300450002004A":
                    tagstrs = "<Icon=\"DPadDown\">";
                    break;
                case "0003000F0002000F":
                    tagstrs = "<Icon=\"JoyStick\">";
                    break;
                case "0003001700020017":
                    tagstrs = "<Icon=\"MButton\">";
                    break;
                case "0003004400020049":
                    tagstrs = "<Icon=\"DPadUp\">";
                    break;
                case "0003001B0002001B":
                    tagstrs = "<Icon=\"GrandStar\">";
                    break;
                case "0003000700020007":
                    tagstrs = "<Icon=\"Star\">";
                    break;
                case "0003002F00020034":
                    tagstrs = "<Icon=\"Tico\">";
                    break;
                case "0003002C00020031":
                    tagstrs = "<Icon=\"1UPMushroom\">";
                    break;
                case "0003000200020002":
                    tagstrs = "<Icon=\"CButton\">";
                    break;
                case "0003001800020018":
                    tagstrs = "<Icon=\"PButton\">";
                    break;
                case "0003000900020009":
                    tagstrs = "<Icon=\"BlueStar\">";
                    break;
                case "0003003400020039":
                    tagstrs = "<Icon=\"Yoshi\">";
                    break;
                case "0003001000020010":
                    tagstrs = "<Icon=\"XIcon\">";
                    break;
                case "0003001100020011":
                    tagstrs = "<Icon=\"Coin\">";
                    break;
                case "0003002000020020":
                    tagstrs = "<Icon=\"GoldCrown\">";
                    break;
                case "0003001900020019":
                    tagstrs = "<Icon=\"ZButton\">";
                    break;
                case "0003002E00020033":
                    tagstrs = "<Icon=\"HarapekoTico\">";
                    break;
                case "0003000D0002000D":
                    tagstrs = "<Icon=\"ArrowDown\">";
                    break;
                case "0003001C0002001C":
                    tagstrs = "<Icon=\"Luigi\">";
                    break;
                case "0003000800020008":
                    tagstrs = "<Icon=\"StarRing\">";
                    break;
                case "0003000000020000":
                    tagstrs = "<Icon=\"AButton\">";
                    break;
                case "0003001E0002001E":
                    tagstrs = "<Icon=\"PurpleCoin\">";
                    break;
                case "0003000100020001":
                    tagstrs = "<Icon=\"BButton\">";
                    break;
                case "0003002100020021":
                    tagstrs = "<Icon=\"Aim\">";
                    break;
                case "0003000400020004":
                    tagstrs = "<Icon=\"Nunchuck\">";
                    break;
                case "0003003300020038":
                    tagstrs = "<Icon=\"MasterTico\">";
                    break;
                case "0003003200020037":
                    tagstrs = "<Icon=\"StopWatch\">";
                    break;
                case "000300460002004B":
                    tagstrs = "<Icon=\"Columa\">";
                    break;
                case "0003001D0002001D":
                    tagstrs = "<Icon=\"PointerYellow\">";
                    break;
                case "000300370002003C":
                    tagstrs = "<Icon=\"Flower\">";
                    break;
                case "0003001A0002001A":
                    tagstrs = "<Icon=\"SilverStar\">";
                    break;

            }

            UTF16BE(tagstrs, list);
            return true;
        }

        public static bool Tag04(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8)
        {

            string tagstrs = "<Unknown=\"Tag04\">";
            switch (bit6)
            {
                case 0x00:
                    tagstrs = "<Size=\"Small\">";
                    break;
                case 0x01:
                    tagstrs = "<Size=\"Normal\">";
                    break;
                case 0x02:
                    tagstrs = "<Size=\"Large\">";
                    break;

            }
            Console.WriteLine(tagstrs + "_てす");
            UTF16BE(tagstrs, list);
            return true;
        }

        public static bool Tag05(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8)
        {
            Bytes2Byte(br);
            Bytes2Byte(br);
            string tagstrs = "<Unknown=\"Tag05\">";
            switch (bit6)
            {
                case 0x00:
                    if (bit8 != 02) break;
                    tagstrs = "</PlayCharacter>";
                    break;
            }
            Console.WriteLine(tagstrs + "_てす");
            UTF16BE(tagstrs, list);
            return true;
        }

        public static bool Tag06(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8)
        {
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            string tagstrs = "<Unknown=\"Tag06\">";
            switch (bit6)
            {
                
                case 0x01:
                    if (bit8 != 08) break;
                    tagstrs = "</StarShipTag>";
                    break;
            }
            UTF16BE(tagstrs, list);
            return true;
        }
        public static bool Tag07(BinaryReader br, FileStream fs, List<byte> list, byte bit5, byte bit6, byte bit7, byte bit8)
        {
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            Bytes2Byte(br);
            string tagstrs = "<Unknown=\"Tag07\">";
            switch (bit6)
            {
                case 0x00:
                    if (bit8 != 08) break;
                    tagstrs = "</ResultGalaxyName>";
                    break;
                case 0x01:
                    if (bit8 != 08) break;
                    tagstrs = "</ResultScenarioName>";
                    break;
            }
            UTF16BE(tagstrs, list);
            return true;
        }

        

        public static byte Bytes2Byte(BinaryReader br) {
            return br.ReadByte();
        }

        public static int Byte2Int(BinaryReader br, int readbyte = 4)
        {
            return Int32.Parse(BitConverter.ToString(br.ReadBytes(readbyte), 0).Replace("-", "").PadLeft(readbyte, '0'), NumberStyles.HexNumber);
        }

        public static Int16 Byte2Short(BinaryReader br, int readbyte = 2)
        {
            return Int16.Parse(BitConverter.ToString(br.ReadBytes(readbyte), 0).Replace("-", "").PadLeft(readbyte, '0'), NumberStyles.HexNumber);
        }

        

        public static uint MSBT_Hash(string label, int num_slots) {
            uint hash = 0;
            foreach (char c in label) {
                hash *= 0x492;
                hash += c;
            }
            return (hash & 0xFFFFFFFF) % (uint)num_slots;
        }

        /// <summary>
        /// BCKファイルのパディング分ﾊﾞｲﾅﾘﾘｰﾄﾞｽﾄﾘｰﾑを進めます<br/>
        /// <remarks>Padding(進めたいﾊﾞｲﾅﾘﾘｰﾄﾞ、ﾌｧｲﾙｽﾄﾘｰﾑの位置(ling型))</remarks>
        /// </summary>
        /// 
        public static void Padding(BinaryReader br, long fs)
        {
            string ps = "";
            for (long k = fs; (k % 16f) != 0; k++)
            {
                ps += Byte2Char(br, 1);
            }
            Debugger.Text(ps);
        }

        public static void Padding_Writer(BinaryWriter bw, long bw_pos)
        {
            int i = 0;
            byte[] pad = StringToBytes("ABABABABABABABABABABABABABABABAB");
            for (long k = bw_pos; (k % 16f) != 0; k++)
            {
                bw.Write(pad[i]);
                i++;
            }
        }

        public static void MSBF_Padding(BinaryReader br, long fs)
        {
            string ps = "";
            for (long k = fs; (k % 16f) != 0; k++)
            {
                ps += Byte2Char(br, 1);
            }
            Debugger.MSBF_Text(ps);
        }

        public static byte[] StringToBytes(string str)
        {
            var bs = new List<byte>();
            for (int i = 0; i < str.Length / 2; i++)
            {
                bs.Add(Convert.ToByte(str.Substring(i * 2, 2), 16));
            }
            return bs.ToArray();
        }

        

        public static byte[] StringToInt32_byte(string str)
        {
            int sh;
            string str2;
            sh = Convert.ToInt32(str, 16);
            str2 = sh.ToString("X8");
            return StringToBytes(str2);

        }

        /// <summary>
        /// ファイルにint32型のNullデータを書き込みます<br/>
        /// <remarks>Null_Writer_Int32(進めたいバイナリライト、繰り返し回数(int型))</remarks>
        /// </summary>
        /// 
        public static void Null_Writer_Int32(BinaryWriter bw , int write_rep_num = 1) {

            //0以上または、int型の最大値までしか選択できないようにする。
            if (write_rep_num < 1) write_rep_num = 1;
            if (Int32.MaxValue < write_rep_num) write_rep_num = Int32.MaxValue;

            for(int i = 0; i<write_rep_num; i++)
            bw.Write(BitConverter.GetBytes(0x00000000));

            //↓旧計算式
            //bw.Write(BitConverter.GetBytes(0x00000000));
        }

        public static void UTF16BE_String_Writer(BinaryWriter bw , string str) {
            bw.Write(Encoding.GetEncoding("unicodeFFFE").GetBytes(str));
        }

        

        public static string String2TagChecker(string str) {
            var oldstr = str.Replace(EN.NewLine, "");
            oldstr = oldstr.Replace("\n", "");
            oldstr = oldstr.Replace("\r", "");
            char[] ch = { '<', '>' };
            string[] oldstrarray = oldstr.Split(ch);
            
            for (int i = 0; i < oldstrarray.Count(); i++) {
                var roopstr = TagChecker(oldstrarray[i]);
                oldstrarray[i] = roopstr;
            }
            
            return string.Join("",oldstrarray);
        }

        public static string TagChecker(string str) {

            byte[] bits;
            switch (str) {
                case "/br":
                    bits = StringToBytes("000A");
                    break;
                case "/End":
                    bits = StringToBytes("0000");
                    break;
                case "Size=\"Small\"":
                    bits = StringToBytes("000E000400000000");
                    break;
                case "Size=\"Normal\"":
                    bits = StringToBytes("000E000400010000");
                    break;
                case "Size=\"Large\"":
                    bits = StringToBytes("000E000400020000");
                    break;
                case "/NCenter":
                    bits = StringToBytes("000E000100010000");
                    break;
                case "/YCenter":
                    bits = StringToBytes("000E000100020000");
                    break;
                case "/XCenter":
                    bits = StringToBytes("000E000100030000");
                    break;
                case "/PlayCharacter":
                    bits = StringToBytes("000E00050000000200CD");
                    break;
                case "/New_Page":
                    bits = StringToBytes("000E000100010000000A");
                    break;
                case "Color=\"Black\"":
                    bits = StringToBytes("000E0000000300020000");
                    break;
                case "Color=\"Red\"":
                    bits = StringToBytes("000E0000000300020001");
                    break;
                case "Color=\"Green\"":
                    bits = StringToBytes("000E0000000300020002");
                    break;
                case "Color=\"Blue\"":
                    bits = StringToBytes("000E0000000300020003");
                    break;
                case "Color=\"Yellow\"":
                    bits = StringToBytes("000E0000000300020004");
                    break;
                case "Color=\"Purple\"":
                    bits = StringToBytes("000E0000000300020005");
                    break;
                case "Color=\"Orange\"":
                    bits = StringToBytes("000E0000000300020006");
                    break;
                case "Color=\"Gray\"":
                    bits = StringToBytes("000E0000000300020007");
                    break;
                case "/Color":
                    bits = StringToBytes("000E000000030002FFFF");
                    break;
                case "Icon=\"CometMedal\"":
                    bits = StringToBytes("000E000300350002003A");
                    break;
                case "Icon=\"SilverCrown\"":
                    bits = StringToBytes("000E000300360002003B");
                    break;
                case "Icon=\"PointerHand\"":
                    bits = StringToBytes("000E0003002500020025");
                    break;

                case "Icon=\"Peach\"":
                    bits = StringToBytes("000E0003002800020028");
                    break;
                case "Icon=\"SilverCrownwJewel\"":
                    bits = StringToBytes("000E0003003F00020044");
                    break;
                case "Icon=\"Begoman\"":
                    bits = StringToBytes("000E0003004100020046");
                    break;
                case "Icon=\"Koopa\"":
                    bits = StringToBytes("000E0003002300020023");
                    break;
                case "Icon=\"Coins\"":
                    bits = StringToBytes("000E0003004300020048");
                    break;
                case "Icon=\"Starbit\"":
                    bits = StringToBytes("000E0003002700020027");
                    break;
                case "Icon=\"BronzeStar\"":
                    bits = StringToBytes("000E0003003D00020042");
                    break;
                case "Icon=\"LifeUpMushroom\"":
                    bits = StringToBytes("000E0003002D00020032");
                    break;
                case "Icon=\"StarPiece\"":
                    bits = StringToBytes("000E0003001500020015");
                    break;
                case "Icon=\"Pointer\"":
                    bits = StringToBytes("000E0003000A0002000A");
                    break;

                case "Icon=\"GreenComet\"":
                    bits = StringToBytes("000E0003001F0002001F");
                    break;
                case "Icon=\"WiiMote\"":
                    bits = StringToBytes("000E0003000300020003");
                    break;
                case "Icon=\"Kinopio\"":
                    bits = StringToBytes("000E000300470002004C");
                    break;
                case "Icon=\"PurpleStarbit\"":
                    bits = StringToBytes("000E0003000B0002000B");
                    break;
                case "Icon=\"Letter\"":
                    bits = StringToBytes("000E0003002900020029");
                    break;
                case "Icon=\"Kuribo\"":
                    bits = StringToBytes("000E0003004200020047");
                    break;
                case "Icon=\"Mario\"":
                    bits = StringToBytes("000E0003002B0002002B");
                    break;
                case "Icon=\"Mario2\"":
                    bits = StringToBytes("000E0003001200020012");
                    break;
                case "Icon=\"DPad\"":
                    bits = StringToBytes("000E0003001300020013");
                    break;
                case "Icon=\"DPadDown\"":
                    bits = StringToBytes("000E000300450002004A");
                    break;

                case "Icon=\"JoyStick\"":
                    bits = StringToBytes("000E0003000F0002000F");
                    break;
                case "Icon=\"MButton\"":
                    bits = StringToBytes("000E0003001700020017");
                    break;
                case "Icon=\"DPadUp\"":
                    bits = StringToBytes("000E0003004400020049");
                    break;
                case "Icon=\"GrandStar\"":
                    bits = StringToBytes("000E0003001B0002001B");
                    break;
                case "Icon=\"Star\"":
                    bits = StringToBytes("000E0003000700020007");
                    break;
                case "Icon=\"Tico\"":
                    bits = StringToBytes("000E0003002F00020034");
                    break;
                case "Icon=\"1UPMushroom\"":
                    bits = StringToBytes("000E0003002C00020031");
                    break;
                case "Icon=\"CButton\"":
                    bits = StringToBytes("000E0003000200020002");
                    break;
                case "Icon=\"PButton\"":
                    bits = StringToBytes("000E0003001800020018");
                    break;
                case "Icon=\"BlueStar\"":
                    bits = StringToBytes("000E0003000900020009");
                    break;

                case "Icon=\"Yoshi\"":
                    bits = StringToBytes("000E0003003400020039");
                    break;
                case "Icon=\"XIcon\"":
                    bits = StringToBytes("000E0003001000020010");
                    break;
                case "Icon=\"Coin\"":
                    bits = StringToBytes("000E0003001100020011");
                    break;
                case "Icon=\"GoldCrown\"":
                    bits = StringToBytes("000E0003002000020020");
                    break;
                case "Icon=\"ZButton\"":
                    bits = StringToBytes("000E0003001900020019");
                    break;
                case "Icon=\"HarapekoTico\"":
                    bits = StringToBytes("000E0003002E00020033");
                    break;
                case "Icon=\"ArrowDown\"":
                    bits = StringToBytes("000E0003000D0002000D");
                    break;
                case "Icon=\"Luigi\"":
                    bits = StringToBytes("000E0003001C0002001C");
                    break;
                case "Icon=\"StarRing\"":
                    bits = StringToBytes("000E0003000800020008");
                    break;
                case "Icon=\"AButton\"":
                    bits = StringToBytes("000E0003000000020000");
                    break;

                case "Icon=\"PurpleCoin\"":
                    bits = StringToBytes("000E0003001E0002001E");
                    break;
                case "Icon=\"BButton\"":
                    bits = StringToBytes("000E0003000100020001");
                    break;
                case "Icon=\"Aim\"":
                    bits = StringToBytes("000E0003002100020021");
                    break;
                case "Icon=\"Nunchuck\"":
                    bits = StringToBytes("000E0003000400020004");
                    break;
                case "Icon=\"MasterTico\"":
                    bits = StringToBytes("000E0003003300020038");
                    break;
                case "Icon=\"StopWatch\"":
                    bits = StringToBytes("000E0003003200020037");
                    break;
                case "Icon=\"Columa\"":
                    bits = StringToBytes("000E000300460002004B");
                    break;
                case "Icon=\"PointerYellow\"":
                    bits = StringToBytes("000E0003001D0002001D");
                    break;
                case "Icon=\"Flower\"":
                    bits = StringToBytes("000E000300370002003C");
                    break;
                case "Icon=\"SilverStar\"":
                    bits = StringToBytes("000E0003001A0002001A");
                    break;
                
                case "/StarShipTag":
                    bits = StringToBytes("000E0006000100080000000000000000");
                    break;
                case "/ResultGalaxyName":
                    bits = StringToBytes("000E0007000000080000000000000000");
                    break;
                case "/ResultScenarioName":
                    bits = StringToBytes("000E0007000100080000000000000001");
                    break;
                default:
                    if (str.Length > 3)
                    {
                        if (str.Substring(0, 4) == "Rubi")
                        {
                            var strs = str.Split('\"');
                            var rubi = Int32.Parse(strs[1]);
                            var target = Int32.Parse(strs[3]);
                            var total = ((target) << 1) + ((rubi) << 1) ;

                            switch (((target) << 1)) {
                                case 2:
                                    total += 2;
                                    break;
                                case 4:
                                    break;
                                case 6:
                                    total -= 2;
                                    break;
                                case 8:
                                    total -= 4;
                                    break;
                                case 10:
                                    total -= 6;
                                    break;
                                default:
                                    break;
                            
                            }
                            
                            var hexnum = ((long)(total) << 32) + ((long)(target) << 17) + ((long)(rubi) << 1);
                            
                            bits = StringToBytes("000E00000000" + hexnum.ToString("X12"));
                            break;
                        }
                    }
                    if (str.Length > 5) {
                        if (str.Substring(0, 6) == "/Timer")
                        {
                            var strs = str.Split('\"');
                            var time = UInt16.Parse(strs[1]);

                            bits = StringToBytes("000E000100000002" + time.ToString("X4"));
                            break;
                        }
                    }
                    return str;
            }

            return Encoding.GetEncoding("unicodeFFFE"/*1201*/).GetString(bits);
        }

        public static void TextBoxInsert(TextBox tb , string str) {
            tb.SelectedText = str;
        }

    }
}
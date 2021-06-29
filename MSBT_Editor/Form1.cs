﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MSBT_Editor.FileSys;
using MSBT_Editor.Formsys;
using MSBT_Editor.Sectionsys;
using System.IO;

namespace MSBT_Editor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
        }

        private static Form1 _form1Instance;


        public static Form1 Form1Instance
        {
            get
            { return _form1Instance; }
            set
            { _form1Instance = value; }
        }

        private void 開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog.Open(1);
        }

        private void 保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0 && (MSBTsys.MSBT_Data.MSBT_All_Data.Item.Count != 0) && MSBTsys.MSBT_Data.MSBT_All_Data.Text.Count != 0)
                Dialog.Save(1);
        }

        private void 上書き保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog.SaveAs();
        }

        private void MSBF開くToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dialog.Open(2);
        }

        private void MSBF保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FLW2 flw2 = new FLW2();
            FEN1 fen1 = new FEN1();

            if (listBox2.Items.Count == 0 || (flw2.Item.Count == 0)) {
                MessageBox.Show("MSBFのFLW2の項目が設定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBox3.Items.Count == 0 || (fen1.Hashes.Count == 0) || (fen1.Item2.Count == 0))
            {
                MessageBox.Show("MSBFのFEN1の項目が設定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Dialog.Save(2);
        }

        private void MSBF上書き保存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FLW2 flw2 = new FLW2();
            FEN1 fen1 = new FEN1();

            if (listBox2.Items.Count == 0 || (flw2.Item.Count == 0))
            {
                MessageBox.Show("MSBFのFLW2の項目が設定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (listBox3.Items.Count == 0 || (fen1.Hashes.Count == 0) || (fen1.Item2.Count == 0))
            {
                MessageBox.Show("MSBFのFEN1の項目が設定されていません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Dialog.SaveAs_Msbf();
        }

        public readonly string[] IconNameJP01 = { "ピーチ", "クッパ", "キノピオ", "マリオ", "マリオ2", "チコ", "ヨッシー", "腹ペコチコ", "ルイージ", "ベビーチコ", "アシストチコ", "ベーゴマン", "クリボー" };
        public readonly string[] IconNameJP02 = { "彗星メダル", "コイン×3", "カラフルスターピース", "イエローチップ", "スターピース紫", "シルバースター", "スター", "グランドスター", "ブロンズスター", "コイン", "パープルコイン", "1UPキノコ", "ライフアップキノコ", "ブルースター", "スターリング", "ヨッシーキャプチャー花" };
        public readonly string[] IconNameJP03 = { "ポインター", "2Pポインター", "ハンドポインター", "Wiiリモコン", "Aボタン", "Bボタン", "Cボタン", "Zボタン", "十字ボタン", "十字ボタン下", "十字ボタン上", "スティック", "ヌンチャク", "照準", "マイナスボタン", "プラスボタン", "×(かける)アイコン", "グリーンコメット", "銀の王冠", "銀の王冠宝石付き", "金の王冠", "手紙", "矢印下", "ストップウォッチ" };
        private void Form1_Load(object sender, EventArgs e)
        {
            Form1.Form1Instance = this;

            comboBox8.Text = Properties.Settings.Default.言語;

            Formsys.Langage.Langage_Check();
            //switch (Properties.Settings.Default.言語) {
            //    case "日本語":
            //        comboBox5.Items.AddRange(IconNameJP01);
            //        comboBox6.Items.AddRange(IconNameJP02);
            //        comboBox7.Items.AddRange(IconNameJP03);
            //        break;
            //    case "EN":
            //        comboBox5.Items.AddRange(IconNameJP01);
            //        comboBox6.Items.AddRange(IconNameJP02);
            //        comboBox7.Items.AddRange(IconNameJP03);
            //        break;
            //    default:
            //        comboBox5.Items.AddRange(IconNameJP01);
            //        comboBox6.Items.AddRange(IconNameJP02);
            //        comboBox7.Items.AddRange(IconNameJP03);
            //        break;

            //}
            comboBox5.Items.AddRange(IconNameJP01);
            comboBox6.Items.AddRange(IconNameJP02);
            comboBox7.Items.AddRange(IconNameJP03);
            //コマンドライン引数を配列で取得する
            string[] files = System.Environment.GetCommandLineArgs();

            if (files.Length > 1) FileSys.Dialog.FileCheck(files[1]);

            
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.textBox25.TextChanged -= new EventHandler(this.textBox25_TextChanged);
            //this.textBox26.TextChanged -= new EventHandler(this.textBox26_TextChanged);
            //if (MSBT_Editor.MSBTsys.MSBT_Data.Txt2_Text_List.Count > 0)
            if (MSBT_Editor.MSBTsys.MSBT_Data.MSBT_All_Data.Text.Count > 0)
            {
                if (listBox1.SelectedIndex < 0) listBox1.SelectedIndex = 0;
                var selectnum = listBox1.SelectedIndex;
                MSBTsys.MSBT_Data.Data_List mad = MSBTsys.MSBT_Data.MSBT_All_Data;
                List<string> nulldata =  MSBTsys.MSBT_Data.atr_nulldata;
                textBox1.Text = mad.Text[selectnum];
                textBox3.Text = mad.Item[selectnum].unknown1.ToString("X2");
                textBox4.Text = mad.Item[selectnum].unknown2.ToString("X2");
                textBox5.Text = mad.Item[selectnum].Dialog_Type.ToString("X2");
                textBox6.Text = mad.Item[selectnum].unknown3.ToString("X2");
                textBox7.Text = mad.Item[selectnum].unknown4.ToString("X4");
                textBox8.Text = mad.Item[selectnum].unknown5.ToString("X2");
                textBox9.Text = mad.Item[selectnum].unknown6.ToString("X2");
                textBox10.Text = mad.Item[selectnum].null_offset.ToString("X8");
                textBox11.Text = nulldata[selectnum];

                var liststrs = listBox1.Text;
                //Console.WriteLine(liststrs+ LBL1.list_name.IndexOf(liststrs)+"_"+LBL1.list_name.Count());
                if (-1 != LBL1.list_name.IndexOf(liststrs))
                {
                    //Console.WriteLine("OK");
                    var unknownlbldata = LBL1.unknownpos[LBL1.list_name.IndexOf(liststrs)].ToString("X");
                    textBox12.Text = LBL1.unknown[LBL1.list_name.IndexOf(liststrs)].ToString("X8");
                }
                else {
                    textBox12.Text = "";
                }
            }
            else 
            {
                textBox1.Text = "null";
            }
            //this.textBox25.TextChanged += new EventHandler(this.textBox25_TextChanged);
            //this.textBox26.TextChanged += new EventHandler(this.textBox26_TextChanged);
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox3);
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox4);
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox5);
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox6);
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox7);
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox8);
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            ATR1.ATR1_Change(textBox9);
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            ATR1.nulldata[listBox1.SelectedIndex] = textBox11.Text;
        }

        private void textBox12_TextChanged(object sender, EventArgs e)
        {
            var liststrs = listBox1.Text;
            if (-1 != LBL1.list_name.IndexOf(liststrs))
            {
                var unknownlbldata = LBL1.unknownpos[LBL1.list_name.IndexOf(liststrs)].ToString("X");
                LBL1.unknown[LBL1.list_name.IndexOf(liststrs)] = Int32.Parse(textBox12.Text,System.Globalization.NumberStyles.HexNumber);
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            if (listBox1.Items.Count != 0)
            {
                MSBTsys.MSBT_Data.MSBT_All_Data.Text[listBox1.SelectedIndex] = textBox1.Text;
            }
        }

        private void AddListText_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0)
            {
                //既存のデータに追加する場合
                if (ListNameText.Text != "")
                {
                    //インスタンス生成と初期化
                    LBL1 lbl1 = new LBL1();
                    lbl1.HashData = new List<LBL1.Hash_Data>();
                    long listcounter = 0;

                    //リストボックス1のデータをハッシュデータ構造体に入れる
                    foreach (var listname in listBox1.Items) {

                        lbl1.HashData.Add(new LBL1.Hash_Data(Calculation_System.MSBT_Hash(listname.ToString(), lbl1.Entries),listcounter));
                        listcounter++;
                    }

                    //ハッシュデータ構造体に入っているデータを編集用の構造体に入れる
                    List<LBL1.Hash_Data> hashlist = new List< LBL1.Hash_Data >(lbl1.HashData);

                    //ハッシュリデータを昇順に並べ替えをする
                    hashlist.Sort((a, b) => a.hash.CompareTo(b.hash));

                    //追加するリスト名をハッシュ値に変換
                    var newhash = Calculation_System.MSBT_Hash(ListNameText.Text, lbl1.Entries);

                    //ハッシュ値を比較して同じ値があった場合は数値を返して、それ以外の場合はdefaultを返す
                    LBL1.Hash_Data equal_flag =  hashlist.LastOrDefault(x => x.hash == newhash);

                    //ハッシュ値がデフォルトの場合、新しいハッシュ値がハッシュリストの値を超える値を返す
                    if (equal_flag.hash == default) equal_flag = hashlist.LastOrDefault(x => x.hash  < newhash);
                    
                    //ハッシュ値を基準に見つかったハッシュ構造体のリスト値に+1した値にリストを挿入する
                    listBox1.Items.Insert((int)equal_flag.listindex +1 , ListNameText.Text);
                    MSBTsys.MSBT_Data.MSBT_All_Data.Text.Insert((int)equal_flag.listindex + 1, "テキスト</End>");
                    MSBTsys.MSBT_Data.MSBT_All_Data.Item.Insert((int)equal_flag.listindex + 1, new ATR1.Item(0x1, 0x0, 0x0, 0x0, 0x0, 0xFF, 0xFF, 0x00));
                    MSBTsys.MSBT_Data.atr_nulldata.Insert((int)equal_flag.listindex + 1, "");

                    //
                    IEnumerable<LBL1.Hash_Data> noduplicates = hashlist.Distinct();
                    hashlist = new List<LBL1.Hash_Data>(noduplicates);
                    var foundhashlist = hashlist.LastOrDefault(x => x.hash == newhash);
                    if (foundhashlist.hash == default) foundhashlist = hashlist.LastOrDefault(x => x.hash -1 < newhash); ;
                    //ラベル情報を追加する
                    var lbl1_newindex = hashlist.LastIndexOf(foundhashlist);
                    Debugger.HashTxt("lbl newindex"+lbl1_newindex.ToString());
                    LBL1.name_offset.Insert(lbl1_newindex,0);
                    LBL1.list_name.Insert(lbl1_newindex, ListNameText.Text);
                    LBL1.unknown.Insert(lbl1_newindex, 0x00000001);
                    LBL1.unknownpos.Insert(lbl1_newindex, 0);
                }
            }
            else {
                //初回にデータを入れる場合
                if (ListNameText.Text != "")
                {
                    //初期化
                    MSBTsys.MSBT_Data.Data_List msbtdatalist = new MSBTsys.MSBT_Data.Data_List();
                    msbtdatalist.Item = new List<ATR1.Item>();
                    msbtdatalist.Text = new List<string>();
                    MSBTsys.MSBT_Data.atr_nulldata = new List<string>() ;
                    LBL1.list_name = new List<string>();
                    LBL1.unknown = new List<int>();
                    LBL1.unknownpos = new List<long>();
                    LBL1.name_offset = new List<int>();

                    //値を入れておく
                    MSBTsys.MSBT_Data.MSBT_All_Data = msbtdatalist;

                    //リストボックス1とMSBTのデータに追加する
                    listBox1.Items.Add(ListNameText.Text);
                    MSBTsys.MSBT_Data.MSBT_All_Data.Text.Add("テキスト</End>");
                    MSBTsys.MSBT_Data.MSBT_All_Data.Item.Add(new ATR1.Item(0x1, 0x0, 0x0, 0x0, 0x0, 0xFF, 0xFF, 0x00));
                    MSBTsys.MSBT_Data.atr_nulldata.Add("");

                    //ラベル情報を追加する
                    LBL1.name_offset.Add(0);
                    LBL1.list_name.Add(ListNameText.Text);
                    LBL1.unknown.Add(1);
                    LBL1.unknownpos.Add(0);
                }
            }
        }

        private void DeleteListText_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count != 0 && (MSBTsys.MSBT_Data.MSBT_All_Data.Item.Count !=0) && MSBTsys.MSBT_Data.MSBT_All_Data.Text.Count != 0)
            {
                var array_list_name = LBL1.list_name.ToArray();
                var array_search = Array.IndexOf(array_list_name, listBox1.SelectedItem);

                if (array_search == -1) return;

                MSBTsys.MSBT_Data.MSBT_All_Data.Item.RemoveRange(listBox1.SelectedIndex,1);
                MSBTsys.MSBT_Data.MSBT_All_Data.Text.RemoveRange(listBox1.SelectedIndex, 1);
                MSBTsys.MSBT_Data.atr_nulldata.RemoveRange(listBox1.SelectedIndex , 1);


                Debugger.HashTxt(array_search.ToString());
                Debugger.HashTxt(LBL1.list_name[array_search]);
                Debugger.HashTxt(LBL1.unknown[array_search].ToString());
                Debugger.HashTxt(LBL1.unknownpos[array_search].ToString());

                LBL1.name_offset.RemoveAt(array_search);
                LBL1.list_name.RemoveAt(array_search);
                LBL1.unknown.RemoveAt(array_search);
                LBL1.unknownpos.RemoveAt(array_search);
                //LBL1.list_name.RemoveRange(array_search , 1);
                //LBL1.unknown.RemoveRange(array_search, 1);
                //LBL1.unknownpos.RemoveRange(array_search, 1);
                var listselect = listBox1.SelectedIndex;
                listBox1.Items.RemoveAt(listselect);
            }
        }

        private void textBox14_TextChanged(object sender, EventArgs e)
        {
            LBL1 lbl1 = new LBL1();
            lbl1.Entries = Int32.Parse(textBox14.Text);
        }

        private void textBox15_TextChanged(object sender, EventArgs e)
        {
            ATR1 atr1 = new ATR1();
            atr1.Entries = Int32.Parse(textBox15.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1 ) return ;

            string color = "";

            switch (comboBox1.Text) {
                case "ブラック":
                    color = "Black";
                    break;
                case "レッド":
                    color = "Red";
                    break;
                case "グリーン":
                    color = "Green";
                    break;
                case "ブルー":
                    color = "Blue";
                    break;
                case "イエロー":
                    color = "Yellow";
                    break;
                case "パープル":
                    color = "Purple";
                    break;
                case "オレンジ":
                    color = "Orange";
                    break;
                case "グレー":
                    color = "Gray";
                    break;
                case "エンドタグ":
                    Calculation_System.TextBoxInsert(textBox1, "</Color>");
                    return;
                default:
                    color = "Black";
                    break;
            }

            string tag = "<Color=\"" + color +"\">";

            Calculation_System.TextBoxInsert(textBox1,tag);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;

            string tag = "";

            switch (comboBox2.Text) {
                case "改行":
                    tag = "</br>" + Environment.NewLine;
                    break;
                case "次のページ":
                    tag = "</New_Page>"+Environment.NewLine + Environment.NewLine;
                    break;
                case "終了":
                    tag = "</End>";
                    break;
                default:
                    tag = "</br>" + Environment.NewLine;
                    break;
            }
            if (tag == "</End>") {
                if (textBox1.Text.IndexOf("</End>") != -1) {
                    MessageBox.Show("</End>が二つ以上あるとデータが"+ "\n\r" +"破損するので挿入をキャンセルしました", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return; 
                }
                
            }
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;

            string tag = "";

            switch (comboBox3.Text)
            {
                case "小":
                    tag = "<Size=\"Small\">";
                    break;
                case "普通":
                    tag = "<Size=\"Normal\">";
                    break;
                case "大":
                    tag = "<Size=\"Large\">";
                    break;
                default:
                    tag = "<Size=\"Normal\">";
                    break;
            }

            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;

            string tag = "";

            switch (comboBox4.Text)
            {
                case "横方向":
                    tag = "</XCenter>";
                    break;
                case "縦方向":
                    tag = "</YCenter>";
                    break;
                default:
                    tag = "</XCenter>";
                    break;
            }

            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            if (textBox16.Text == "" || textBox17.Text == "") return;
            string furigana = textBox16.Text;
            string kanji = textBox17.Text;
            string tag = "<Rubi=\"" + furigana + "\" Target=\"" + kanji + "\">";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void textBox16_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void textBox17_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void textBox18_KeyPress(object sender, KeyPressEventArgs e)
        {
            //0～9と、バックスペース以外の時は、イベントをキャンセルする
            if ((e.KeyChar < '0' || '9' < e.KeyChar) && e.KeyChar != '\b')
            {
                e.Handled = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            if (textBox18.Text == "" ) return;
            string time = textBox18.Text;
            string tag = "</Timer=\"" + time + "\">";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</PlayCharacter>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</StarShipTag>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</ResultGalaxyName>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</ResultScenarioName>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button11_Click(object sender, EventArgs e)
        {

            //if (listBox1.Items.Count < 1) return;
            //if (comboBox5.Items.Count == -1) return;
            //if (comboBox5.SelectedIndex == -1) comboBox5.SelectedIndex = 0 ;
            string[] IconTag = { "<Icon=\"Peach\">", "<Icon=\"Koopa\">", "<Icon=\"Kinopio\">", "<Icon=\"Mario\">", "<Icon=\"Mario2\">", "<Icon=\"Tico\">", "<Icon=\"Yoshi\">", "<Icon=\"HarapekoTico\">", "<Icon=\"Luigi\">", "<Icon=\"MasterTico\">", "<Icon=\"Columa\">", "<Icon=\"Begoman\">", "<Icon=\"Kuribo\">" };
            Calculation_System.TextBoxTagAdder(listBox1,textBox1,comboBox5,IconTag);
            //int index = comboBox5.SelectedIndex;
            //string tag = IconTag[index];
            //Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            //if (listBox1.Items.Count < 1) return;
            //if (comboBox6.Items.Count == -1) return;
            //if (comboBox6.SelectedIndex == -1) comboBox6.SelectedIndex = 0;
            string[] IconTag = { "<Icon=\"CometMedal\">", "<Icon=\"Coins\">", "<Icon=\"Starbit\">", "<Icon=\"StarPiece\">", "<Icon=\"PurpleStarbit\">", "<Icon=\"SilverStar\">", "<Icon=\"Star\">", "<Icon=\"GrandStar\">", "<Icon=\"BronzeStar\">", "<Icon=\"Coin\">", "<Icon=\"PurpleCoin\">", "<Icon=\"1UPMushroom\">", "<Icon=\"LifeUpMushroom\">", "<Icon=\"BlueStar\">", "<Icon=\"StarRing\">", "<Icon=\"Flower\">" };
            Calculation_System.TextBoxTagAdder(listBox1, textBox1, comboBox6, IconTag);
            //int index = comboBox6.SelectedIndex;
            //string tag = IconTag[index];
            //Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            string[] IconTag = { "<Icon=\"Pointer\">", "<Icon=\"PointerYellow\">", "<Icon=\"PointerHand\">", "<Icon=\"WiiMote\">", "<Icon=\"AButton\">", "<Icon=\"BButton\">", "<Icon=\"CButton\">", "<Icon=\"ZButton\">", "<Icon=\"DPad\">", "<Icon=\"DPadDown\">", "<Icon=\"DPadUp\">", "<Icon=\"JoyStick\">", "<Icon=\"Nunchuck\">", "<Icon=\"Aim\">", "<Icon=\"MButton\">", "<Icon=\"PButton\">", "<Icon=\"XIcon\">", "<Icon=\"GreenComet\">", "<Icon=\"SilverCrown\">", "<Icon=\"SilverCrownwJewel\">", "<Icon=\"GoldCrown\">", "<Icon=\"Letter\">", "<Icon=\"ArrowDown\">", "<Icon=\"StopWatch\">" };
            Calculation_System.TextBoxTagAdder(listBox1, textBox1, comboBox7, IconTag);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</Year01>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</Year02>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</Hour>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</Minutes>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</Seconds>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (listBox1.Items.Count < 1) return;
            string tag = "</AfterTheDecimalPoint>";
            Calculation_System.TextBoxInsert(textBox1, tag);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (toolStripStatusLabel2.Text == " ") return;
            if (textBox13.Text == "") return;
            string appPath = System.Reflection.Assembly.GetExecutingAssembly().Location;
            string msbtname = Path.GetFileNameWithoutExtension(toolStripStatusLabel2.Text);
            string textpath = Path.Combine(Path.GetDirectoryName(appPath),msbtname+".txt");
            Console.WriteLine(textpath);

            File.WriteAllText(textpath, textBox13.Text);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox25.TextChanged -= new EventHandler(this.textBox25_TextChanged);
            this.textBox26.TextChanged -= new EventHandler(this.textBox26_TextChanged);

            if (listBox2.Items.Count == 0) return;
            var index = listBox2.SelectedIndex;
            if (index == -1) index = 0;
            FLW2 flw2 = new FLW2();
            Console.WriteLine(index);
            textBox19.Text = flw2.Item[index].TypeCheck.ToString("X4");
            textBox20.Text = flw2.Item[index].Unknown1.ToString("X4");
            textBox21.Text = flw2.Item[index].Unknown2.ToString("X4");
            textBox22.Text = flw2.Item[index].Unknown3.ToString("X4");
            textBox23.Text = flw2.Item[index].Unknown4.ToString("X4");
            textBox24.Text = flw2.Item[index].Unknown5.ToString("X4");

            switch (flw2.Item[index].TypeCheck.ToString("X4")) {
                case "0001":
                    label25.Text = "グループ番号";
                    label26.Text = "Msbtテキストlist番号";
                    label27.Text = "FLW2オフセット";
                    label28.Text = "不明5";
                    if (Properties.Settings.Default.言語 == "日本語") break;
                    label25.Text = "Group number";
                    label26.Text = "Msbt text list number";
                    label27.Text = "FLW2 offset";
                    label28.Text = "Unknown5";
                    break;
                case "0002":
                    label25.Text = "0002固定";
                    label26.Text = "不明3";
                    label27.Text = "不明4";
                    label28.Text = "分岐先オフセット";
                    if (Properties.Settings.Default.言語 == "日本語") break;
                    label25.Text = "0002 fixed";
                    label26.Text = "Unknown3";
                    label27.Text = "Unknown4";
                    label28.Text = "Branch offset";
                    break;
                case "0004":
                    label25.Text = "ジャンプ先";
                    label26.Text = "不明3";
                    label27.Text = "不明4";
                    label28.Text = "不明5";
                    if (Properties.Settings.Default.言語 == "日本語") break;
                    label25.Text = "Jump destination";
                    label26.Text = "Unknown3";
                    label27.Text = "Unknown4";
                    label28.Text = "Unknown5";
                    break;
                case "0003":
                    label25.Text = "イベント番号";
                    label26.Text = "不明3";
                    label27.Text = "不明4";
                    label28.Text = "不明5";
                    if (Properties.Settings.Default.言語 == "日本語") break;
                    label25.Text = "Event Number";
                    label26.Text = "Unknown3";
                    label27.Text = "Unknown4";
                    label28.Text = "Unknown5";
                    break;
                default:
                    label25.Text = "不明2";
                    label26.Text = "不明3";
                    label27.Text = "不明4";
                    label28.Text = "不明5";
                    if (Properties.Settings.Default.言語 == "日本語") break;
                    label25.Text = "Unknown2";
                    label26.Text = "Unknown3";
                    label27.Text = "Unknown4";
                    label28.Text = "Unknown5";
                    break;

            }

            //if (flw2.Branch_List_No == null) return;

            Console.WriteLine("/////////////listchec   " + listBox2.Items.Count);

            var blnc = flw2.Branch_List_No.Count*2;
            var bnc = flw2.Branch_No.Count;

            Console.WriteLine(blnc+"_ひかくー_"+bnc);
            if (blnc != bnc) {
                this.textBox25.TextChanged += new EventHandler(this.textBox25_TextChanged);
                this.textBox26.TextChanged += new EventHandler(this.textBox26_TextChanged);
                return;
            }
            if (-1 == flw2.Branch_List_No.IndexOf(index)) {

                textBox25.Text = "";
                textBox26.Text = "";
                this.textBox25.TextChanged += new EventHandler(this.textBox25_TextChanged);
                this.textBox26.TextChanged += new EventHandler(this.textBox26_TextChanged);
                return;
            }
            textBox27.AppendText(Environment.NewLine + "selectlist");
            //var brannum = flw2.Item[index].Unknown5;
            textBox27.AppendText(Environment.NewLine + flw2.Branch_No[flw2.Branch_List_No.IndexOf(index)].ToString("X4")+"___"+ flw2.Branch_List_No.IndexOf(index));

            textBox25.Text = flw2.Branch_No[(flw2.Branch_List_No.IndexOf(index)*2)].ToString("X4");
            textBox26.Text = flw2.Branch_No[(flw2.Branch_List_No.IndexOf(index)*2)+1].ToString("X4");

            //textBox25.Text = flw2.Branch_No[brannum].ToString("X4");
            //textBox26.Text = flw2.Branch_No[brannum+1].ToString("X4");

            this.textBox25.TextChanged += new EventHandler(this.textBox25_TextChanged);
            this.textBox26.TextChanged += new EventHandler(this.textBox26_TextChanged);
        }

        private void textBox19_TextChanged(object sender, EventArgs e)
        {
            this.listBox2.SelectedIndexChanged -= new EventHandler(this.listBox2_SelectedIndexChanged);
            FLW2.FLW2_Item_Change(listBox2,textBox19);
            this.listBox2.SelectedIndexChanged += new EventHandler(this.listBox2_SelectedIndexChanged);
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
            this.listBox2.SelectedIndexChanged -= new EventHandler(this.listBox2_SelectedIndexChanged);
            FLW2.FLW2_Item_Change(listBox2, textBox20);
            this.listBox2.SelectedIndexChanged += new EventHandler(this.listBox2_SelectedIndexChanged);
        }

        private void textBox21_TextChanged(object sender, EventArgs e)
        {
            FLW2.FLW2_Item_Change(listBox2, textBox21);
        }

        private void textBox22_TextChanged(object sender, EventArgs e)
        {
            FLW2.FLW2_Item_Change(listBox2, textBox22);
        }

        private void textBox23_TextChanged(object sender, EventArgs e)
        {
            FLW2.FLW2_Item_Change(listBox2, textBox23);
        }

        private void textBox24_TextChanged(object sender, EventArgs e)
        {
            FLW2.FLW2_Item_Change(listBox2, textBox24);
        }

        private void textBox25_TextChanged(object sender, EventArgs e)
        {
            
            textBox27.AppendText(Environment.NewLine + "25text");
            FLW2.FLW2_FlowType2_Branch(listBox2, textBox25);
            Console.WriteLine("★" + listBox2.Items.Count);

        }

        private void textBox26_TextChanged(object sender, EventArgs e)
        {
            FLW2.FLW2_FlowType2_Branch(listBox2, textBox26);
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            string[] fileName = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            var filecount = fileName.Count();

            if (filecount == 2) { 
                var path1 = Path.GetExtension(fileName[0]);
                var path2 = Path.GetExtension(fileName[1]);

                if (path1 == path2) {
                    MessageBox.Show("2つファイルを読み込む場合"+"\n\r"+"MSBTとMSBFの組み合わせのみです", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            switch (filecount) {
                case 1:
                    Dialog.FileCheck(fileName[0]);
                    break;
                case 2:
                    Dialog.FileCheck(fileName[0]);
                    Dialog.FileCheck(fileName[1]);
                    break;
                default:
                    MessageBox.Show("3つ以上のファイルはドロップできません", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop)) e.Effect = DragDropEffects.Copy;
            else e.Effect = DragDropEffects.None;
        }

        private void textBox25_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox26_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void textBox28_TextChanged(object sender, EventArgs e)
        {
            FEN1.FEN1_Item_Change(listBox3 , textBox28);
        }

        private void textBox29_TextChanged(object sender, EventArgs e)
        {
            FEN1.FEN1_Item_Change(listBox3, textBox29);
        }

        private void listBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.textBox28.TextChanged -= new EventHandler(this.textBox28_TextChanged);
            this.textBox29.TextChanged -= new EventHandler(this.textBox29_TextChanged);

            if (listBox3.Items.Count == 0) return;
            var index = listBox3.SelectedIndex;
            if (index == -1) index = 0;

            FEN1 fen1 = new FEN1();
            var hashes = Calculation_System.MSBT_Hash(fen1.Item2[index].tagname , 0x3B);

            //textBox28.Text = fen1.Item1[index].tagflag.ToString("X8");
            textBox28.Text = fen1.Hashes[index].tagflag.ToString("X8");
            textBox29.Text = fen1.Item2[index].tagnum.ToString("X8");

            if (listBox2.Items.Count > fen1.Item2[index].tagnum) listBox2.SelectedIndex = fen1.Item2[index].tagnum;

            this.textBox28.TextChanged += new EventHandler(this.textBox28_TextChanged);
            this.textBox29.TextChanged += new EventHandler(this.textBox29_TextChanged);

        }

        private void textBox30_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox31_TextChanged(object sender, EventArgs e)
        {

        }

        private void button21_Click(object sender, EventArgs e)
        {
            FLW2 flw2 = new FLW2();
            if (listBox2.Items.Count != 0)
            {
                listBox2.Items.Add(Langage.FLW2_List_Langage(4));
                flw2.Item.Add(new FLW2.flw2_item(4,0,0,0,0,0));
                listBox2.EndUpdate();
            }
            else
            {
                listBox2.Items.Add(Langage.FLW2_List_Langage(4));
                flw2.Item = new  List<FLW2.flw2_item>();
                flw2.Item.Add(new FLW2.flw2_item(4, 0, 0, 0, 0, 0));
                flw2.Branch_List_No = new List<int>();
                flw2.Branch_No = new List<short>();
                listBox2.EndUpdate();
            }
        }

        private void button22_Click(object sender, EventArgs e)
        {
            FLW2 flw2 = new FLW2();
            
            if (listBox2.Items.Count != 0 && (flw2.Item.Count != 0) && (flw2.Branch_List_No != null) && (flw2.Branch_No != null))
            {
                

                var listselect = listBox2.SelectedIndex;
                listBox2.Items.RemoveAt(listselect);
                if (flw2.Item[listselect].TypeCheck == 2) {
                    flw2.Branch_No.RemoveAt((flw2.Branch_List_No.IndexOf(listselect))*2);
                    flw2.Branch_List_No.RemoveAt(flw2.Branch_List_No.IndexOf(listselect));
                }
                flw2.Item.RemoveAt(listselect);
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            FEN1 fen1 = new FEN1();
            if (listBox3.Items.Count != 0)
            {
                if (textBox31.Text != "")
                {
                    listBox3.Items.Add(textBox31.Text);
                    fen1.Item1.Add(new FEN1.Element(1,0));
                    var item2count = fen1.Item2.Count;
                    fen1.Item2.Add(new FEN1.ElementTag(textBox31.Text,item2count));
                    var hash = Calculation_System.MSBT_Hash(textBox31.Text,0x3B);
                    fen1.Hashes.Add(new FEN1.Hash_And_Unknown(1, hash));

                }
            }
            else
            {
                if (textBox31.Text != "")
                {
                    fen1.Item1 = new List<FEN1.Element>();
                    fen1.Item2 = new List<FEN1.ElementTag>();
                    fen1.Hashes = new List<FEN1.Hash_And_Unknown>();
                    

                    listBox3.Items.Add(textBox31.Text);
                    fen1.Item1.Add(new FEN1.Element(1, 0));
                    
                    fen1.Item2.Add(new FEN1.ElementTag(textBox31.Text, 0));

                    var hash = Calculation_System.MSBT_Hash(textBox31.Text, 0x3B);
                    fen1.Hashes.Add(new FEN1.Hash_And_Unknown(1, hash));
                }
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            FEN1 fen1 = new FEN1();

            if (listBox3.Items.Count != 0 && (fen1.Item1.Count != 0) && (fen1.Item2.Count != 0) && (fen1.Hashes.Count != 0))
            {
                Console.WriteLine( fen1.Item1.Count());
                Console.WriteLine(fen1.Item2.Count());
                Console.WriteLine(fen1.Hashes.Count());

                

                var listselect = listBox3.SelectedIndex;
                listBox3.Items.RemoveAt(listselect);
                
                fen1.Item1.RemoveAt(listselect);
                fen1.Item2.RemoveAt(listselect);
                fen1.Hashes.RemoveAt(listselect);

                
            }
        }

        private void ListNameText_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox8_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox8.SelectedIndex)
            {
                case 0:
                    Properties.Settings.Default.言語 = "日本語";
                    
                    break;
                case 1:
                    Properties.Settings.Default.言語 = "EN";
                    break;
                default:
                    Properties.Settings.Default.言語 = "日本語";
                    break;
            }

            Properties.Settings.Default.Save();
            Formsys.Langage.Langage_Check();
        }
    }
}

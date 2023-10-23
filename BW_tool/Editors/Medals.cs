/*
 * Created by SharpDevelop.
 * User: suloku
 * Date: 03/03/2016
 * Time: 19:38
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BW_tool
{
	/// <summary>
	/// Description of Medals.
	/// </summary>
	public partial class Medals : Form
	{
		MEDAL medals;
		public Medals()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			red_panel.Visible = false;
			medals =  new MEDAL(MainForm.save.getBlock(68));
			indexbox.SelectedIndex = 0;
			
			update_date();
			/*
			day.Value = medals.Day;
			month.Value = medals.Month;
			year.Value = medals.Year;
			*/
			update_medal_state();
			flag4box.Checked = medals.Flag4;
			
			//MessageBox.Show(medal_date.Value.Year.ToString());
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
		{
			if (indexbox.SelectedIndex == 0)
			{
				indexbox.SelectedIndex = 1;
			}
			else if (indexbox.SelectedIndex == 8)
			{
				indexbox.SelectedIndex = 9;
			}
			else if (indexbox.SelectedIndex == 107)
			{
				indexbox.SelectedIndex = 108;
			}
			else if (indexbox.SelectedIndex == 164)
			{
				indexbox.SelectedIndex = 165;
			}
			else if (indexbox.SelectedIndex == 189)
			{
				indexbox.SelectedIndex = 190;
			}

			if (indexbox.SelectedIndex > 0 && indexbox.SelectedIndex < 8)
			{
				medals.Index = (int)indexbox.SelectedIndex - 1;
			}
			else if (indexbox.SelectedIndex > 8 && indexbox.SelectedIndex < 107)
			{
				medals.Index = (int)indexbox.SelectedIndex - 2;
			}
			else if (indexbox.SelectedIndex > 107 && indexbox.SelectedIndex < 164)
			{
				medals.Index = (int)indexbox.SelectedIndex - 3;
			}
			else if (indexbox.SelectedIndex > 164 && indexbox.SelectedIndex < 189)
			{
				medals.Index = (int)indexbox.SelectedIndex - 4;
			}
			else if (indexbox.SelectedIndex > 189)
			{
				medals.Index = (int)indexbox.SelectedIndex - 5;
			}
			
			update_date();

			/*
			day.Value = medals.Day;
			month.Value = medals.Month;
			year.Value = medals.Year;
			*/
			update_medal_state();
			flag4box.Checked = medals.Flag4;
		}
		
		void DayValueChanged(object sender, EventArgs e)
		{
			//medals.Day = (int)day.Value;
		}
		void MonthValueChanged(object sender, EventArgs e)
		{
		/*
			medals.Month = (int)month.Value;
			switch((int)month.Value)
			{
				case 01:
				case 03:
				case 05:
				case 07:
				case 08:
				case 10:
				case 12:
					day.Maximum = 31;
					break;
				case 04:
				case 06:
				case 09:
				case 11:
					day.Maximum = 30;
					break;
				case 02:
					if (DateTime.IsLeapYear((int)year.Value))
						day.Maximum = 29;
					else
						day.Maximum = 28;
					break;
				
			}
		*/
		}
		void YearValueChanged(object sender, EventArgs e)
		{
		/*
			medals.Year = (int)year.Value;
			if(month.Value == 2)
			{
					if (DateTime.IsLeapYear((int)year.Value))
						day.Maximum = 29;
					else
						day.Maximum = 28;
			}
			*/
		}
		void Exit_butClick(object sender, EventArgs e)
		{
			this.Close();
		}
		void Saveexit_butClick(object sender, EventArgs e)
		{
			MainForm.save.setBlock(medals.Data, 68);
			this.Close();
		}
		void Flag4boxCheckedChanged(object sender, EventArgs e)
		{
			medals.Flag4 = flag4box.Checked;
			update_hex();
		}
		
		void update_date()
		{
			if (medals.Year != 0 && medals.Month != 0 && medals.Day != 0)
			{
				red_panel.Visible = false;
				medal_date.Value = new DateTime(2000+medals.Year, medals.Month, medals.Day);
				obtained.Checked = true;
			}
			else
			{
				red_panel.Visible = true;
				obtained.Checked = false;
			}
		}
		void Medal_dateValueChanged(object sender, EventArgs e)
		{
			medals.Year = medal_date.Value.Year-2000;
			medals.Month = medal_date.Value.Month;
			medals.Day = medal_date.Value.Day;
			
			update_date();
			update_hex();

		}
		void Delete_butClick(object sender, EventArgs e)
		{
			medals.Year = 0;
			medals.Month = 0;
			medals.Day = 0;
			medals.Flag1 = false;
			medals.Flag2 = false;
			medals.Flag3 = false;
			medals.Flag4 = false;
			update_date();
			update_medal_state();
			flag4box.Checked = medals.Flag4;
		}
		void update_hex()
		{
			date_hex.Text = (medals.medal_bytes & 0x0000FFFF).ToString("X4");
			flag_hex.Text = ((medals.medal_bytes & 0xFFFF0000)>>4).ToString("X4");
		}

		private void medalState_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (medalState.SelectedIndex == 0)
			{
				medals.Flag1 = false;
				medals.Flag2 = false;
				medals.Flag3 = false;
			}
			else if (medalState.SelectedIndex == 1)
			{
				medals.Flag1 = true;
				medals.Flag2 = false;
				medals.Flag3 = false;
			}
			else if (medalState.SelectedIndex == 2)
			{
				medals.Flag1 = false;
				medals.Flag2 = true;
				medals.Flag3 = false;
			}
			else if (medalState.SelectedIndex == 3)
			{
				medals.Flag1 = true;
				medals.Flag2 = true;
				medals.Flag3 = false;
			}
			else if (medalState.SelectedIndex == 4)
			{
				medals.Flag1 = false;
				medals.Flag2 = false;
				medals.Flag3 = true;
			}
			update_hex();
		}

		private void update_medal_state()
		{
			if (!medals.Flag1 && !medals.Flag2 && !medals.Flag3) medalState.SelectedIndex = 0;
			else if (medals.Flag1 && !medals.Flag2 && !medals.Flag3) medalState.SelectedIndex = 1;
			else if (!medals.Flag1 && medals.Flag2 && !medals.Flag3) medalState.SelectedIndex = 2;
			else if (medals.Flag1 && medals.Flag2 && !medals.Flag3) medalState.SelectedIndex = 3;
			else if (!medals.Flag1 && !medals.Flag2 && medals.Flag3) medalState.SelectedIndex = 4;
		}

		public class MEDAL
		{
			internal int MedalSize = 4;
			internal int Size = MainForm.save.getBlockLength(68);
			
			public byte[] Data;
			public MEDAL(byte[] data = null)
			{
				Data = data ?? new byte[Size];
			}
			private int index = 0;
			public int Index {
				get { return index/MedalSize; }
				set { if ((value >= 0) && (value < 256)){ index = (int)(value*MedalSize);} } 
			}
					
			private UInt16 build_date(int day, int month, int year)
			{
				return (UInt16) ( ((day & 0x1F) << 11) | ((month & 0x0F) << 7) | (year & 0x7F)  );
			}
					
			public UInt32 medal_bytes { get { return (BitConverter.ToUInt32(Data, index));} }

			public int Day {
				get { return (BitConverter.ToUInt16(Data, index)) >> 11; }
				set { BitConverter.GetBytes(build_date(value, Month, Year)).CopyTo(Data, index); } }
			public int Month {
				get { return (BitConverter.ToUInt16(Data, index) & 0x0780) >> 7; }
				set { BitConverter.GetBytes(build_date(Day, value, Year)).CopyTo(Data, index); } }
			public int Year {
				get { return (BitConverter.ToUInt16(Data, index) & 0x007F); }
				set { BitConverter.GetBytes(build_date(Day, Month, value)).CopyTo(Data, index); } }
			public UInt16 Flags {
				get { return BitConverter.ToUInt16(Data, index+0x02); }
				set { BitConverter.GetBytes((UInt16)value).CopyTo(Data, index+0x2); } }
					
			public bool Flag1 {
				get { return Convert.ToBoolean(Flags&0x1); }
				//set { Flags = (UInt16)(Convert.ToByte(value) | (Flags & ~(0x1))); } }
				set { if (value == true) Flags = (UInt16)( Flags|0x1); else Flags = (UInt16)(Flags & ~(0x1)); } }
			public bool Flag2 {
				get { return Convert.ToBoolean(Flags&0x2); }
				set { if (value == true) Flags = (UInt16)(Flags|0x2); else Flags = (UInt16)(Flags & ~(0x2)); } }
			public bool Flag3 {
				get { return Convert.ToBoolean(Flags&0x4); }
				set { if (value == true) Flags = (UInt16)(Flags|0x4); else Flags = (UInt16)(Flags & ~(0x4)); } }
			public bool Flag4 {
				get { return Convert.ToBoolean(Flags&0x8); }
				set { if (value == true) Flags = (UInt16)(Flags|0x8); else Flags = (UInt16)(Flags & ~(0x8)); } }
		}
	}
}

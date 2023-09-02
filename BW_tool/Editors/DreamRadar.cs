/*
 * Created by SharpDevelop.
 * User: sergi
 * Date: 23/03/2017
 * Time: 13:29
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BW_tool
{
	/// <summary>
	/// Description of DreamRadar.
	/// </summary>
	public partial class DreamRadar : Form
	{
		DRKEY drkey;
		int drkeyblock = 72;
		DRA dra;
		DRB drb;
		
		public DreamRadar()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			drkey =  new DRKEY(MainForm.save.getBlock(drkeyblock));
			dra = new DRA(MainForm.save.dslinkA_get());
			drb = new DRB(PKX5.cryptoXor32Array(MainForm.save.dslinkB_get(), 0, 0x7C, 0x7C)); //Get 3DS link data decrypted
			
			if (dra.received == false)
			{
				MessageBox.Show("Warning! There's unreceived data in the savegame!");
				dra.key = drb.EncKey^drkey.FLAGS;//This makes editing the data possible without messing up the current encryption
			}

			if (drb.illegal == true)
			{
				allmode.Checked = true;
				set_all_list();
				
			}
			else
			{
				legitmode.Checked = true;
				set_legal_list();
			}

			load_data();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void Exit_butClick(object sender, EventArgs e)
		{
			this.Close();
		}
		void Saveexit_butClick(object sender, EventArgs e)
		{
			set_data();
			MainForm.save.setBlock(drkey.Data, 72);
			MainForm.save.dslinkA_set(dra.Data);
			MainForm.save.dslinkB_set(PKX5.cryptoXor32Array(drb.Data, 0, 0x7C, 0x7C));
			this.Close();
		}
		void Clean_butClick(object sender, EventArgs e) //Right now this button is disabled as we know how to edit all the data
		{
			drkey.reinit();
			dra.reinit();
			drb.reinit();
			
			MainForm.save.setBlock(drkey.Data, 72);
			MainForm.save.dslinkA_set(dra.Data);
			MainForm.save.dslinkB_set(PKX5.cryptoXor32Array(drb.Data, 0, 0x7C, 0x7C));
			this.Close();
		}
		
		void set_legal_list()
		{
			pkm1.Items.Clear();
			pkm2.Items.Clear();
			pkm3.Items.Clear();
			pkm4.Items.Clear();
			pkm5.Items.Clear();
			pkm6.Items.Clear();
			pkm1.Items.AddRange(TEXT.drpklist);
			pkm2.Items.AddRange(TEXT.drpklist);
			pkm3.Items.AddRange(TEXT.drpklist);
			pkm4.Items.AddRange(TEXT.drpklist);
			pkm5.Items.AddRange(TEXT.drpklist);
			pkm6.Items.AddRange(TEXT.drpklist);
			item1.Items.Clear();
			item2.Items.Clear();
			item3.Items.Clear();
			item4.Items.Clear();
			item5.Items.Clear();
			item6.Items.Clear();
			item1.Items.AddRange(TEXT.dritemlist);
			item2.Items.AddRange(TEXT.dritemlist);
			item3.Items.AddRange(TEXT.dritemlist);
			item4.Items.AddRange(TEXT.dritemlist);
			item5.Items.AddRange(TEXT.dritemlist);
			item6.Items.AddRange(TEXT.dritemlist);
		}

		void set_all_list()
		{
			pkm1.Items.Clear();
			pkm2.Items.Clear();
			pkm3.Items.Clear();
			pkm4.Items.Clear();
			pkm5.Items.Clear();
			pkm6.Items.Clear();
			pkm1.Items.AddRange(TEXT.pkmlist);
			pkm2.Items.AddRange(TEXT.pkmlist);
			pkm3.Items.AddRange(TEXT.pkmlist);
			pkm4.Items.AddRange(TEXT.pkmlist);
			pkm5.Items.AddRange(TEXT.pkmlist);
			pkm6.Items.AddRange(TEXT.pkmlist);
			item1.Items.Clear();
			item2.Items.Clear();
			item3.Items.Clear();
			item4.Items.Clear();
			item5.Items.Clear();
			item6.Items.Clear();
			item1.Items.AddRange(TEXT.itemlist);
			item2.Items.AddRange(TEXT.itemlist);
			item3.Items.AddRange(TEXT.itemlist);
			item4.Items.AddRange(TEXT.itemlist);
			item5.Items.AddRange(TEXT.itemlist);
			item6.Items.AddRange(TEXT.itemlist);
		}
		int poke2legit(int species)
		{
			if (legitmode.Checked == true)
			{
				int i = 0;
				for (i = 0; i<19; i++)
				{
					if (species == drb.legit_pk_index[i])
						return i;
				}
			}
			return species;
		}
		int item2legit(int item)
		{
			if (legitmode.Checked == true)
			{
				int i = 0;
				for (i = 0; i<24; i++)
				{
					if (item == drb.legit_item_index[i])
						return i;
				}
			}
			return item;
		}
		void load_data()
		{
			pkm1.SelectedIndex = poke2legit(drb.get_poke_species(0));
			pkm2.SelectedIndex = poke2legit(drb.get_poke_species(1));
			pkm3.SelectedIndex = poke2legit(drb.get_poke_species(2));
			pkm4.SelectedIndex = poke2legit(drb.get_poke_species(3));
			pkm5.SelectedIndex = poke2legit(drb.get_poke_species(4));
			pkm6.SelectedIndex = poke2legit(drb.get_poke_species(5));
			
			item1.SelectedIndex = item2legit(drb.get_item_id(0));
			item2.SelectedIndex = item2legit(drb.get_item_id(1));
			item3.SelectedIndex = item2legit(drb.get_item_id(2));
			item4.SelectedIndex = item2legit(drb.get_item_id(3));
			item5.SelectedIndex = item2legit(drb.get_item_id(4));
			item6.SelectedIndex = item2legit(drb.get_item_id(5));
				
			pkmgnd1.SelectedIndex = drb.get_poke_gender(0);
			pkmgnd2.SelectedIndex = drb.get_poke_gender(1);
			pkmgnd3.SelectedIndex = drb.get_poke_gender(2);
			pkmgnd4.SelectedIndex = drb.get_poke_gender(3);
			pkmgnd5.SelectedIndex = drb.get_poke_gender(4);
			pkmgnd6.SelectedIndex = drb.get_poke_gender(5);
			
			itemcnt1.Value = drb.get_item_amount(0);
			itemcnt2.Value = drb.get_item_amount(1);
			itemcnt3.Value = drb.get_item_amount(2);
			itemcnt4.Value = drb.get_item_amount(3);
			itemcnt5.Value = drb.get_item_amount(4);
			itemcnt6.Value = drb.get_item_amount(5);
			
			legend1.SelectedIndex = drb.get_legend(0);
			legend2.SelectedIndex = drb.get_legend(1);
			legend3.SelectedIndex = drb.get_legend(2);
			legend4.SelectedIndex = drb.get_legend(3);
			legend5.SelectedIndex = drb.get_legend(4);
			legend6.SelectedIndex = drb.get_legend(5);
			legend7.SelectedIndex = drb.get_legend(6);
			legend8.SelectedIndex = drb.get_legend(7);
			
			flag0.Checked = drkey.Tornadus;
			flag1.Checked = drkey.Thundurus;
			flag2.Checked = drkey.Landorus;
			flag3.Checked = drkey.Dialga;
			flag4.Checked = drkey.Palkia;
			flag5.Checked = drkey.Giratina;
			flag6.Checked = drkey.HoOh;
			flag7.Checked = drkey.Lugia;
		}
		
		void set_data()
		{
			//Get the correct intial key before changing the flags
			drb.EncKey = dra.key^drkey.FLAGS;
			
			//Set the flags
			drkey.Tornadus = flag0.Checked;
			drkey.Thundurus = flag1.Checked;
			drkey.Landorus = flag2.Checked;
			drkey.Dialga = flag3.Checked;
			drkey.Palkia = flag4.Checked;
			drkey.Giratina = flag5.Checked;
			drkey.HoOh = flag6.Checked;
			drkey.Lugia = flag7.Checked;
			
			drb.clean_data();//Set everything to 0 first
			//Legendary slots
			switch (legend1.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 0);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 0);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 0);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 0);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 0);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 0);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 0);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 0);
					break;
				default:
					drb.set_legend(0, 0);
					break;
			};
			switch (legend2.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 1);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 1);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 1);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 1);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 1);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 1);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 1);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 1);
					break;
				default:
					drb.set_legend(0, 1);
					break;
			};
			switch (legend3.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 2);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 2);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 2);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 2);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 2);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 2);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 2);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 2);
					break;
				default:
					drb.set_legend(0, 2);
					break;
			};
			switch (legend4.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 3);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 3);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 3);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 3);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 3);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 3);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 3);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 3);
					break;
				default:
					drb.set_legend(0, 3);
					break;
			};
			switch (legend5.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 4);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 4);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 4);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 4);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 4);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 4);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 4);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 4);
					break;
				default:
					drb.set_legend(0, 4);
					break;
			};
			switch (legend6.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 5);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 5);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 5);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 5);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 5);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 5);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 5);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 5);
					break;
				default:
					drb.set_legend(0, 5);
					break;
			};
			switch (legend7.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 6);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 6);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 6);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 6);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 6);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 6);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 6);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 6);
					break;
				default:
					drb.set_legend(0, 6);
					break;
			};
			switch (legend8.SelectedIndex)
			{
				case 1:
					drb.set_legend(DRB.Tornadus, 7);
					break;
				case 2:
					drb.set_legend(DRB.Thundurus, 7);
					break;
				case 3:
					drb.set_legend(DRB.Landorus, 7);
					break;
				case 4:
					drb.set_legend(DRB.Dialga, 7);
					break;
				case 5:
					drb.set_legend(DRB.Palkia, 7);
					break;
				case 6:
					drb.set_legend(DRB.Giratina, 7);
					break;
				case 7:
					drb.set_legend(DRB.HoOh, 7);
					break;
				case 8:
					drb.set_legend(DRB.Lugia, 7);
					break;
				default:
					drb.set_legend(0, 7);
					break;
			};
			if (legitmode.Checked == true)
			{
				drb.set_poke(drb.legit_pk_index[pkm1.SelectedIndex], pkmgnd1.SelectedIndex, 0);
				drb.set_poke(drb.legit_pk_index[pkm2.SelectedIndex], pkmgnd2.SelectedIndex, 1);
				drb.set_poke(drb.legit_pk_index[pkm3.SelectedIndex], pkmgnd3.SelectedIndex, 2);
				drb.set_poke(drb.legit_pk_index[pkm4.SelectedIndex], pkmgnd4.SelectedIndex, 3);
				drb.set_poke(drb.legit_pk_index[pkm5.SelectedIndex], pkmgnd5.SelectedIndex, 4);
				drb.set_poke(drb.legit_pk_index[pkm6.SelectedIndex], pkmgnd6.SelectedIndex, 5);
				
				drb.set_item((int)itemcnt1.Value, drb.legit_item_index[item1.SelectedIndex], 0);
				drb.set_item((int)itemcnt2.Value, drb.legit_item_index[item2.SelectedIndex], 1);
				drb.set_item((int)itemcnt3.Value, drb.legit_item_index[item3.SelectedIndex], 2);
				drb.set_item((int)itemcnt4.Value, drb.legit_item_index[item4.SelectedIndex], 3);
				drb.set_item((int)itemcnt5.Value, drb.legit_item_index[item5.SelectedIndex], 4);
				drb.set_item((int)itemcnt6.Value, drb.legit_item_index[item6.SelectedIndex], 5);
			}
			else
			{
				drb.set_poke(pkm1.SelectedIndex, pkmgnd1.SelectedIndex, 0);
				drb.set_poke(pkm2.SelectedIndex, pkmgnd2.SelectedIndex, 1);
				drb.set_poke(pkm3.SelectedIndex, pkmgnd3.SelectedIndex, 2);
				drb.set_poke(pkm4.SelectedIndex, pkmgnd4.SelectedIndex, 3);
				drb.set_poke(pkm5.SelectedIndex, pkmgnd5.SelectedIndex, 4);
				drb.set_poke(pkm6.SelectedIndex, pkmgnd6.SelectedIndex, 5);
				
				drb.set_item((int)itemcnt1.Value, item1.SelectedIndex, 0);
				drb.set_item((int)itemcnt2.Value, item2.SelectedIndex, 1);
				drb.set_item((int)itemcnt3.Value, item3.SelectedIndex, 2);
				drb.set_item((int)itemcnt4.Value, item4.SelectedIndex, 3);
				drb.set_item((int)itemcnt5.Value, item5.SelectedIndex, 4);
				drb.set_item((int)itemcnt6.Value, item6.SelectedIndex, 5);
			}
			
			int i = 0;
			for(i=0;i<drb.Size;i++)
			{
				if (drb.Data[i] != 0)
					dra.received = false; //If we have any data, mark as available to be received
			}
			
			//This is what Dream Radar does, not doing this makes the data be recognized as corrupted.
			dra.unknown1 = 0;
			dra.key = 0;

		}
		void AllmodeCheckedChanged(object sender, EventArgs e)
		{
			if (allmode.Checked == true)
			{
				set_all_list();
				
			}
			else
			{
				set_legal_list();
			}

			load_data();
		}
		void LegitmodeCheckedChanged(object sender, EventArgs e)
		{
			if (allmode.Checked == true)
			{
				set_all_list();
				
			}
			else
			{
				set_legal_list();
			}

			load_data();
		}
		void Flag0CheckedChanged(object sender, EventArgs e)
		{
	
		}
	}
	
	public class DRKEY
	    {
			internal int Size = MainForm.save.getBlockLength(72);
	
	        public byte[] Data;
	        public DRKEY(byte[] data = null)
	        {
	            Data = data ?? new byte[Size];
	        }
	        
	        public UInt32 unknown1 //Not sure how this is used/updated by 3DS link data block
	        {
	            get { return BitConverter.ToUInt32(Data, 0x00); }
	            set { BitConverter.GetBytes((UInt32)value).CopyTo(Data, 0x00); }
	        }
	        
	        public byte FLAGS { get { return Data[0x04]; } set { Data[0x04] = value; } }
			public bool Tornadus { get { return (FLAGS & (1 << 0)) == 1 << 0; } set { FLAGS = (byte)(FLAGS & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
	        public bool Thundurus  { get { return (FLAGS & (1 << 1)) == 1 << 1; } set { FLAGS = (byte)(FLAGS & ~(1 << 1) | (value ? 1 << 1 : 0)); } }
	        public bool Landorus { get { return (FLAGS & (1 << 2)) == 1 << 2; } set { FLAGS = (byte)(FLAGS & ~(1 << 2) | (value ? 1 << 2 : 0)); } }
	        public bool Dialga { get { return (FLAGS & (1 << 3)) == 1 << 3; } set { FLAGS = (byte)(FLAGS & ~(1 << 3) | (value ? 1 << 3 : 0)); } }
	        public bool Palkia { get { return (FLAGS & (1 << 4)) == 1 << 4; } set { FLAGS = (byte)(FLAGS & ~(1 << 4) | (value ? 1 << 4 : 0)); } }
	        public bool Giratina { get { return (FLAGS & (1 << 5)) == 1 << 5; } set { FLAGS = (byte)(FLAGS & ~(1 << 5) | (value ? 1 << 5 : 0)); } }
	        public bool HoOh { get { return (FLAGS & (1 << 6)) == 1 << 6; } set { FLAGS = (byte)(FLAGS & ~(1 << 6) | (value ? 1 << 6 : 0)); } }
	        public bool Lugia { get { return (FLAGS & (1 << 7)) == 1 << 7; } set { FLAGS = (byte)(FLAGS & ~(1 << 7) | (value ? 1 << 7 : 0)); } }
	        
	        //This value is set to 0x01 when 3DS Link has been used at least once and seems to remain that way
	        public bool used { get { return (Data[0x08] & (1 << 0)) == 1 << 0; } set { Data[0x08] = (byte)(Data[0x08] & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
	        
	        public void reinit() //Call this to clean all data so legendaries can be received again. Must be used with the other classes reinit() for it to work
	        {
	        	unknown1 = 0;
	        	FLAGS = 0;
	        	used = false;
	        }
 
		}
	public class DRA
	    {
			internal int Size = 0x10;
			private UInt32 ident = 0x43524746;
	
	        public byte[] Data;
	        public DRA(byte[] data = null)
	        {
	            Data = data ?? new byte[Size];
	        }
	        
	        //This value is set to 0x01 when 3DS Link has data to be received and 0x00 when no data/data already received
	        public bool received { get { return (Data[0x00] & (1 << 0)) == 1 << 0; } set { Data[0x00] = (byte)(Data[0x00] & ~(1 << 0) | (value ? 1 << 0 : 0)); } }
	        
	        public UInt32 unknown1 //derived from offset 0x25E00 (0x00000000 if the Pokemon were not yet received)
	        {
	            get { return BitConverter.ToUInt32(Data, 0x04); }
	            set { BitConverter.GetBytes((UInt32)value).CopyTo(Data, 0x04); }
	        }
	        public UInt32 Ident //always 0x43524746 (CRGF)
	        {
	            get { return BitConverter.ToUInt32(Data, 0x08); }
	            //set { BitConverter.GetBytes((UInt32)value).CopyTo(Data, 0x08); }
	            set { BitConverter.GetBytes(ident).CopyTo(Data, 0x08); }
	        }
	        public UInt32 key //Encryption Key used for the next transfer from DR (0x00000000 if the Pokemon were not yet received) --> Also seems to hold the lengedary flags!
	        {
	            get { return BitConverter.ToUInt32(Data, 0x0C); }
	            set { BitConverter.GetBytes((UInt32)value).CopyTo(Data, 0x0C); }
	        }
	        
	        public void reinit() //Call this to clean all data so legendaries can be received again. Must be used with the other classes reinit() for it to work
	        {
	        	received = true;
	        	unknown1 = 0;
	        	Ident = ident;
	        	key = 0;
	        }
  
		}
	public class DRB
	    {
			public int Size = 0x80;
	
	        public byte[] Data;
	        public DRB(byte[] data = null)
	        {
	            Data = data ?? new byte[Size];
	            
	            int i = 0;
            
	            //Read the legendary values      
	            for (i=0; i<8;i++)
	            {
	            	legendaries[i] = BitConverter.ToUInt32(Data, 0x00 + i*4);
	            }
	            
	            //Read pokemon
	            for (i=0; i<6;i++)
	            {
	            	pokes[i] = BitConverter.ToUInt32(Data, 0x20 + i*4);
	            }
	            
	            //Read items
	            for (i=0; i<6;i++)
	            {
	            	items[i] = BitConverter.ToUInt32(Data, 0x38 + i*4);
	            }
	            
	            //Check legality of data
	            illegal = false;
				int j = 0;
	            for (i=0; i<6;i++)
	            {
			        for (j=0;j<19;j++)
			        {
			        	if (get_poke_species(i) == legit_pk_index[j])
			        	{
			        		break; //Found valid index, continue next pokémon
			        	}
			        	else if ( (get_poke_species(i) != legit_pk_index[j]) && j == 18) //Couldn't find valid species index
			        	{
			        		illegal = true;
			        	}
			        }
			        if (illegal == true) // No need to continue
			        	break;
	            }
	            
	            if (illegal == false)
	            {
		            for (i=0; i<6;i++)
		            {
				    	j = 0;
				        for (j=0;j<24;j++)
				        {
				        	if (get_item_id(i) == legit_item_index[j])
				        	{
				        		break; //Found valid index, continue next item
				        	}
				        	else if ( (get_item_id(i) != legit_item_index[j]) && j == 23) //Couldn't find valid item index
				        	{
				        		illegal = true;
				        	}
				        }
				        if (illegal == true) // No need to continue
				        	break;
		            }
	            }
	            
	        }
	        
			public const UInt32 Tornadus = 0x80808080;
			public const UInt32 Thundurus = 0x92567284;
			public const UInt32 Landorus = 0x87643567;
			public const UInt32 Dialga = 0x96436763;
			public const UInt32 Palkia = 0x43867368;
			public const UInt32 Giratina = 0x17693572;
			public const UInt32 HoOh = 0x44798367;
			public const UInt32 Lugia = 0x96636983;
			
			private UInt32[] legendaries  = new UInt32[8];
			
			public int[] legit_pk_index = new int[]{0, 79, 120, 137, 163, 174, 175, 213, 238, 280, 333, 374, 425, 436, 442, 447, 479, 517, 561};
			public int[] legit_item_index = new int[]{0, 72, 73, 74, 75, 51, 154, 28, 29, 80, 109, 81, 82, 83, 84, 85, 91, 93, 270, 275, 538, 44, 50, 221};
			
			public bool illegal;
			
			public int get_legend(int index) //Simple wrapper for the legendary values
			{
				int legend = 0;
				switch (legendaries[index])
				{
					case Tornadus:
						legend = 1;
						break;
					case Thundurus:
						legend = 2;
						break;
					case Landorus:
						legend = 3;
						break;
					case Dialga:
						legend = 4;
						break;
					case Palkia:
						legend = 5;
						break;
					case Giratina:
						legend = 6;
						break;
					case HoOh:
						legend = 7;
						break;
					case Lugia:
						legend = 8;
						break;
					default:
						legend = 0;
						break;
				}
				return legend;
			}
			
			public void set_legend(UInt32 legend, int index)
			{
	            BitConverter.GetBytes(legend).CopyTo(Data, 0x00 + index*4);
			}
			UInt32[] pokes  = new UInt32[6];
			UInt32[] items  = new UInt32[6];
			
			public int get_poke_species(int index)
			{
				return (int)pokes[index] >> 16;
			}
			public int get_poke_gender(int index)
			{
				return (int)pokes[index] & 0x0000000F;
			}
			
			public void set_poke(int species, int gender, int index)
			{
				int finalgender = gender;
				
				//Set fixed gender species regardless of user choice
				if (species == 0)
				{
					finalgender = 0;
				}
				else
				{	
					int i = 0;
					for (i=0;i<male_only.Length;i++)
					{
						if (species == male_only[i])
						    finalgender = 0;
					}
					for (i=0;i<female_only.Length;i++)
					{
						if (species == female_only[i])
						    finalgender = 1;
					}
					for (i=0;i<genderless.Length;i++)
					{
						if (species == genderless[i])
						    finalgender = 2;
					}
				}
				
				
				pokes[index] = (UInt32)(finalgender | (species << 16));
				BitConverter.GetBytes(pokes[index]).CopyTo(Data, 0x20 + index*4);

			}
			
			public int get_item_id(int index)
			{
				return (int)items[index] >> 16;
			}
			public int get_item_amount(int index)
			{
				//Even though item amount is stored as u16, only the least byte is used, so maximum is 255
				if (((int)items[index] & 0x0000FFFF) <255){
					return (int)items[index] & 0x0000FFFF;
				}else return 255;
			}
			
			public void set_item(int amount, int id, int index)
			{
				if (amount == 0 && id > 0) //Put 1 of an item if it's 0
				{
					items[index] = (UInt32)((id << 16) | 1);
				}
				else if (id == 0)
				{
					items[index] = 0;
				}
				else
				{
					items[index] = (UInt32)((id << 16) | amount);
				}
				BitConverter.GetBytes(items[index]).CopyTo(Data, 0x38 + index*4);
			}
			
			public UInt32 EncKey
	        {
	            get { return BitConverter.ToUInt32(Data, 0x7C); }
	            set { BitConverter.GetBytes((UInt32)value).CopyTo(Data, 0x7C); }
	        }
  
	        public void reinit() //Call this to clean all data so legendaries can be received again. Must be used with the other classes reinit() for it to work
	        {
	        	int i = 0;
	            for (i=0; i<Size;i++)
	            {
	            	Data[i] = 0;
	            }
	        }
	        
	        public void clean_data()
	        {
	        	int i = 0;
	            for (i=0; i<Size-4;i++)
	            {
	            	Data[i] = 0;
	            }
	        }
	        
	        //Some arrays for gender filter
			int[] male_only = new int[]{ 32, 33, 34, 106, 107, 128, 237, 313, 414, 475, 538, 539, 627, 628, 236, 381, 641, 642, 645 };
			int[] female_only = new int[]{ 29 ,113, 115, 124, 241, 242, 314, 413, 416, 478, 548, 549, 629, 630, 30, 31, 238, 380, 440, 488 };
			int[] genderless = new int[]{ 81, 82, 100, 101, 120, 121, 137, 233, 292, 337, 338, 343, 344, 374, 375, 376, 436, 437, 462, 474, 479, 489, 490, 599, 600, 601, 615, 622, 623, 132, 144, 145, 146, 150, 151, 201, 243, 244, 245, 249, 250, 251, 377, 378, 379, 382, 383, 384, 385, 386, 480, 481, 482, 483, 484, 486, 487, 491, 492, 493, 494, 638, 639, 640, 643, 644, 646, 647, 648, 649 };
			
		}
}

//3DS Link structure research by BlackShark: https://projectpokemon.org/forums/forums/topic/40323-white-2-dream-radar-flags/

/*
Save Offset  0x25E00
0x00  unknown (changes when the data is received)
0x04  legendary received flags
0x08  1 after DR data was received the first time, does not increase

Legendary received flags
Bit Pokemon
0   Tornadus
1   Thundurus
2   Landorus
3   Dialga
4   Palkia
5   Giratina
6   Ho-Oh
7   Lugia
======================================================================
 
Save Offset  0x7F000 (the Dream Radar itself only touches this area!)
 
0x00         received/not received flag (1 after the Pokemon were received)
0x01 - 0x03  0x000000
0x04 - 0x07  unknown (derived from offset 0x25E00) (0x00000000 if the Pokemon were not yet received)
0x08 - 0x0B  always 0x43524746 (CRGF)
0x0C - 0x0F  Encryption Key used for the next transfer from DR (0x00000000 if the Pokemon were not yet received) --> Also seems to hold the lengedary flags!
0x10 - 0x11  CRC-16-CCITT over 0x00 - 0x0F
0x12 - 0x13  0x0000
0x14 - 0x33  legendary Pokemon (see values below) (8 x 4 Bytes)
0x34 - 0x4B  normal Pokemon (6 x 4 Bytes)
0x4C - 0x63  Items (6 x 4 Bytes)
0x63 - 0x8F  probably unused (all zero)
0x90 - 0x93  Decryption Key
0x94 - 0x95  CRC-16-CCITT over 0x14 - 0x93
0x96 - 0x97  0x0000

Legendary Pokemon Values
0x80808080  Tornadus
0x92567284  Thundurus
0x87643567  Landorus
0x96436763  Dialga
0x43867368  Palkia
0x17693572  Giratina
0x44798367  Ho-Oh
0x96636983  Lugia
 
Pokemon Structure
0x00         Gender (0 - Male, 1 - Female, 2 - Genderless)
0x01         unknown/unused
0x02 - 0x03  Species ID
 
Item Structure
0x00 - 0x01  Quantity
0x02 - 0x03  Item ID

*/

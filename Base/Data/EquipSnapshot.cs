using System;
using Terraria;

namespace AssortedCrazyThings.Base.Data
{
	/// <summary>
	/// Class holding the equip IDs, used to perform checks for equipment more easily
	/// </summary>
	public class EquipSnapshot
	{
		public int head = -1;
		public int body = -1;
		public int legs = -1;

		public EquipSnapshot(int head = -1, int body = -1, int legs = -1)
		{
			this.head = head;
			this.body = body;
			this.legs = legs;
		}

		public EquipSnapshot(EquipSnapshot other)
		{
			head = other.head;
			body = other.body;
			legs = other.legs;
		}

		public void Reset()
		{
			head = -1;
			body = -1;
			legs = -1;
		}

		public void TakeSnapshot(Player player)
		{
			head = player.head;
			body = player.body;
			legs = player.legs;
		}

		public void CopyFrom(EquipSnapshot other)
		{
			head = other.head;
			body = other.body;
			legs = other.legs;
		}

		/// <summary>
		/// Returned value will contain the values of self if match, -1 entries if no match
		/// </summary>
		public EquipSnapshot GetDifference(EquipSnapshot other)
		{
			return new EquipSnapshot(
				head == other.head ? head : -1,
				body == other.body ? body : -1,
				legs == other.legs ? legs : -1
				);
		}

		public EquipSnapshot GetMasked(EquipSnapshot mask)
		{
			var maskedSelf = new EquipSnapshot(this);
			if (mask.head == -1)
			{
				maskedSelf.head = -1;
			}
			if (mask.body == -1)
			{
				maskedSelf.body = -1;
			}
			if (mask.legs == -1)
			{
				maskedSelf.legs = -1;
			}

			return maskedSelf;
		}

		public override bool Equals(object obj)
		{
			if (obj is not EquipSnapshot other)
			{
				return false;
			}

			return head == other.head && body == other.body && legs == other.legs;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(head, body, legs);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TrafficSim.gui
{
	public class ObjectLinker
	{

		public object target;
		public string property;

		public ObjectLinker(object target, string property)
		{
			this.target = target;
			this.property = property;
		}

		public override string ToString()
		{
			var prop = target.GetType().GetProperty(property, BindingFlags.Public | BindingFlags.Instance);
			return prop.GetValue(target).ToString();
		}



	}
}

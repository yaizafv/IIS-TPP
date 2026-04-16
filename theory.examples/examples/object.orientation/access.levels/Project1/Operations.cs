using myData;

namespace n
{
	class Operations
	{
		public static void AccesibilityChecks()
		{
			Cache c = new Cache();
			c.Put("", "");  //OK
			//c.Clear();	//Error
			//c.Grow();		//Error
			c.HitRate();    //OK
		}
	}
}

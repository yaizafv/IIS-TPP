using myData;

namespace myCustomData
{
	class CustomCache : Cache
	{
		public static void AccesibilityChecks()
		{
			Cache c = new Cache();
			c.Put("", "");  //OK
			//c.Clear();	//Error
			//c.Grow();		//Error
			c.HitRate();    //OK

			CustomCache c2 = new CustomCache();
			c2.Put("", "");	//OK
			//c2.Clear();	//Error
			c2.Grow();      //OK
			c2.HitRate();   //OK
		}
	}
}

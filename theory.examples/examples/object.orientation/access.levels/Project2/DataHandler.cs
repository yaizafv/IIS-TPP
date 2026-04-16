namespace myData
{
	class DataHandler
	{
		public static void AccesibilityChecks()
		{
			Cache c = new Cache();
			c.Put("", "");  //OK
			//c.Clear();	//Error
			//c.Grow();		//Error
			//c.HitRate();    //Error
		}
	}
}

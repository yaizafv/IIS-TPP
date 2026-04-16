using System.Collections.Generic;

/// <summary>
/// A thread-safe ReaderWriterLockSlim optimized table to hold updatable translation strings
/// </summary>
public class TranslationTable
{
    private System.Threading.ReaderWriterLockSlim cacheLock = new System.Threading.ReaderWriterLockSlim();
    private Dictionary<string, string> innerCache = new Dictionary<string, string>();

    public System.Threading.ReaderWriterLockSlim CacheLock { get => cacheLock; set => cacheLock = value; }
    public Dictionary<string, string> InnerCache { get => innerCache; set => innerCache = value; }

    /// <summary>
    /// Counts how many strings are contained within the table
    /// </summary>
    public int StringCount
    { get { return InnerCache.Count; } }

    /// <summary>
    /// Translates an string
    /// </summary>
    /// <param name="key">String in the original language</param>
    /// <returns>String in the target language</returns>
public string Translate(string key)
{
    CacheLock.EnterReadLock();
    try
    {
        return InnerCache[key];
    }
    finally
    {
        CacheLock.ExitReadLock();
    }
}

    /// <summary>
    /// Puts a translation in the table
    /// </summary>
    /// <param name="key">String in the original language</param>
    /// <param name="value">String in the target language</param>
    public void AddTranslation(string key, string value)
    {
        CacheLock.EnterWriteLock();
        try
        {
            InnerCache.Add(key, value);
        }
        finally
        {
            //Always frees a write lock, avoiding deadlocks
            CacheLock.ExitWriteLock();
        }
    }
    /// <summary>
    /// Puts a translation in the table
    /// </summary>
    /// <param name="key">String in the original language</param>
    /// <param name="value">String in the target language</param>
    /// <param name="timeout">Ms. to wait until the operation expires</param>
    /// <returns>If the operation could be completed within the specified timeout</returns>
    public bool AddTranslationWithTimeout(string key, string value, int timeout)
    {
        if (CacheLock.TryEnterWriteLock(timeout))
        {
            try
            {
                InnerCache.Add(key, value);
            }
            finally
            {
                //Always frees a write lock, avoiding deadlocks
                CacheLock.ExitWriteLock();
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary>
    /// Puts a translation in the table or updates an existing one
    /// </summary>
    /// <param name="key">String in the original language</param>
    /// <param name="value">String in the target language</param>
    public void AddOrUpdateTranslation(string key, string value)
    {
//We are reading the value, and add or update it only if it does not exist or has a different value than the one specified.
//Therefore we use a read lock upgradeable to a write one
CacheLock.EnterUpgradeableReadLock();
try
{
    string result = null;
    if (InnerCache.TryGetValue(key, out result))
    {
        //Value exist and is the same -> no more work to do
        if (result == value) return;
        else
        {
            //Update the translation entering a write lock
            CacheLock.EnterWriteLock();
            try
            {
                InnerCache[key] = value;
            }
            finally
            {
                //Always frees a write lock, avoiding deadlocks
                CacheLock.ExitWriteLock();
            }
        }
    }
            else
            {
                //Adds a translation entering a write lock
                CacheLock.EnterWriteLock();
                try
                {
                    InnerCache.Add(key, value);
                }
                finally
                {
                    //Always frees a write lock, avoiding deadlocks
                    CacheLock.ExitWriteLock();
                }
                return;
            }
}
finally
{
    //Always frees a upgradeable read lock, avoiding deadlocks if other thread tries to use one
    CacheLock.ExitUpgradeableReadLock();
}
    }

    /// <summary>
    /// Deletes the specified translation
    /// </summary>
    /// <param name="key">Translation to delete</param>
    public void DeleteTranslation(string key)
    {
        CacheLock.EnterWriteLock();
        try
        {
            InnerCache.Remove(key);
        }
        finally
        {
            //Always frees a write lock, avoiding deadlocks
            CacheLock.ExitWriteLock();
        }
    }


    /// <summary>
    /// Free resources when objects are deleted by the GC
    /// </summary>
    ~TranslationTable()
    {
        if (CacheLock != null) CacheLock.Dispose();
    }
}

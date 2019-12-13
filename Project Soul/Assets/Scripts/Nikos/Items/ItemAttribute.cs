[System.Serializable]
public class ItemAttribute
{
    public string name;
    public int value;

    public ItemAttribute()
    {

    }

    public ItemAttribute(string name, int value)
    {
        this.name = name;
        this.value = value;
    }
}
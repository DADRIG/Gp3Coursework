package org.me.myparserstuff;

public class Widget
{
	private String title ;
	private String description;
	
	
	public Widget()
	{
		title = "";
		description = "";
		
	}
	
	public Widget(String atitle,String adescription)
	{
		title = atitle;
		description = adescription;
		
	}
		
	
	public void setTitle(String atitle)
	{
		title = atitle;
	}
	
	public void setDescription (String adescription)
	{
		description = adescription;
	}
	
	
	
	public String getTitle()
	{
		return title;
	}
	
	public String getDescription()
	{
		return description;
	}
	
	
	
	public String toString()
	{
		String temp;
		
		temp = getTitle() + "\n" + getDescription();
		
		return temp;
		
	}

}

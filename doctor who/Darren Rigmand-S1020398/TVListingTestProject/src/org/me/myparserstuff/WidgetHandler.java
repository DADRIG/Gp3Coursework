
package org.me.myparserstuff;

import java.util.LinkedList;



import org.xml.sax.Attributes;
import org.xml.sax.SAXException;
import org.xml.sax.helpers.DefaultHandler;



public class WidgetHandler extends DefaultHandler 
{
	
	   private StringBuffer buffer = new StringBuffer();
	   private LinkedList<Widget> alist;
	
	    
	    private Widget aWidget;

	    
	    private boolean parsedProgramme = false;

	    
	    //Finds Tag to start parsing.
	    @Override
	    public void startElement(String namespaceURI, String localName,
	            String qName, Attributes atts) throws SAXException 
	    {
	        buffer.setLength(0);
	        if (qName.equalsIgnoreCase("channel") || localName.equalsIgnoreCase("channel"))
	        {
	        	
	        	alist = new LinkedList<Widget>();
	        	 
	        }
	        if (qName.equals("title") || localName.equals("title")) 
	        {
	           aWidget = new Widget();
	        }

	    }
	    
	    
        //Finds Tag to cease parsing.
		@Override
	    public void endElement(String uri, String localName, String qName)throws SAXException 
	    {
	    	
	        
	        
	        if (parsedProgramme == false)
	        {
	        	if (qName.equalsIgnoreCase("title") || localName.equalsIgnoreCase("title")) 
	        	{
		            aWidget.setTitle(buffer.toString());
		        }
	        	else if (qName.equalsIgnoreCase("description") || localName.equalsIgnoreCase("description"))
	        	{
		            aWidget.setDescription(buffer.toString());
		        }
	        	
	        	
		        
	        }
	        
            //Adds a programme to the linked list.
	        if (localName.equals("title")) 
	        {
	            parsedProgramme = false;
	            alist.add(aWidget);
	            
	        }

	       
	        	      
	    }
		
		
		@Override
		public void characters (char[] ch, int start, int length)
		{
			buffer.append(ch, start, length);
		}
	    
	   
	    // returns the list to the Activity Class   
	    public LinkedList<Widget> retrieveWidgets() 
	    {

	        return alist;
	    }
	    
	    
}

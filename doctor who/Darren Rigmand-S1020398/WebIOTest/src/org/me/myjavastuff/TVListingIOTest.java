package org.me.myjavastuff;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;

public class TVListingIOTest 
{
	
	/**
	 * @param args
	 */
	public static void main(String[] args) 
	{
		// TODO Auto-generated method stub
		InputStream anInStream;				
		String listingURL="http://bleb.org/tv/data/rss.php?ch=bbc1_scotland&day=0";
		try
		{
			anInStream = OpenHttpConnection(listingURL);
			//System.out.println(anInStream.toString());
			InputStreamReader in= new InputStreamReader(anInStream);
			BufferedReader bin= new BufferedReader(in);
			String text=bin.readLine();
			System.out.println("The values after BufferedReader:"+text);
			String text1 =  listingString(listingURL);
			System.out.println("TV Listing String is " + text1);
		}
		catch (IOException ae)
		{
			System.out.println(ae.toString());			
		}
		
	} // End of main
	
	 private static InputStream OpenHttpConnection(String urlString)throws IOException
	 {
	    	InputStream in = null;
	    	int response = -1;
	    	URL url = new URL(urlString);
	    	URLConnection conn = url.openConnection();
	    	if (!(conn instanceof HttpURLConnection))
	    			throw new IOException("Not an HTTP connection");
	    	try
	    	{
	    		HttpURLConnection httpConn = (HttpURLConnection) conn;
	    		httpConn.setAllowUserInteraction(false);
	    		httpConn.setInstanceFollowRedirects(true);
	    		httpConn.setRequestMethod("GET");
	    		httpConn.connect();
	    		response = httpConn.getResponseCode();
	    		if (response == HttpURLConnection.HTTP_OK)
	    		{
	    			in = httpConn.getInputStream();	   
	    			System.out.println("All ok so far \n");
	    		}
	    		else
	    		{
	    			System.out.println("Not ok so far \n");
	    		}
	    		System.out.println("Response \n" + response);
	    	}
	    	catch (Exception ex)
	    	{
	    		throw new IOException("Error connecting");
	    	}
	    	return in;
	 } // End of OpenHttpConnection
	 
	 private static String listingString(String urlString)throws IOException
	 {
		 	String result = "";
	    	InputStream anInStream = null;
	    	int response = -1;
	    	URL url = new URL(urlString);
	    	URLConnection conn = url.openConnection();
	    	if (!(conn instanceof HttpURLConnection))
	    			throw new IOException("Not an HTTP connection");
	    	try
	    	{
	    		HttpURLConnection httpConn = (HttpURLConnection) conn;
	    		httpConn.setAllowUserInteraction(false);
	    		httpConn.setInstanceFollowRedirects(true);
	    		httpConn.setRequestMethod("GET");
	    		httpConn.connect();
	    		response = httpConn.getResponseCode();
	    		if (response == HttpURLConnection.HTTP_OK)
	    		{
	    			anInStream = httpConn.getInputStream();
	    			InputStreamReader in= new InputStreamReader(anInStream);
	    			BufferedReader bin= new BufferedReader(in);	 
	    			// Input is over multiple lines
	    			String line = new String();
	    			while (( (line = bin.readLine())) != null)
	    			{
	    				result = result + "\n" + line;
	    			}
	    		}
	    	}
	    	catch (Exception ex)
	    	{
	    			throw new IOException("Error connecting");
	    	}
	    	return result;
	    }

	} // End of listingString

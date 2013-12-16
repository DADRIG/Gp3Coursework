package org.me.myandroidstuff;

import java.io.BufferedReader;

import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.StringReader;
import java.net.HttpURLConnection;
import java.net.URL;
import java.net.URLConnection;
import java.util.LinkedList;

import org.me.myparserstuff.Widget;
import org.me.myparserstuff.WidgetHandler;
import org.me.myparserstuff.XmlParser;
import org.xml.sax.InputSource;

import android.app.Activity;
import android.app.ProgressDialog;
import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemSelectedListener;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ImageView;
import android.widget.Spinner;
import android.widget.TextView;
import android.widget.ViewSwitcher;

public class TVListingTestActivity extends Activity implements OnClickListener,
		OnItemSelectedListener {
	private TextView response;
	private TextView errorText;
	private String result;
	private String tvListingURL = "";
	private Spinner DaySelector;
	private Spinner ChannelSelector;
	private String day = "";
	private ImageView img;
	private Button button;
	View Switcher;
	private ViewSwitcher switchscreen;

	/** Called when the activity is first created. */

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.main);

		response = (TextView) findViewById(R.id.urlResponse);

		DaySelector = (Spinner) findViewById(R.id.dayspinner);
		ChannelSelector = (Spinner) findViewById(R.id.channelspinner);
		img = (ImageView) findViewById(R.id.channelPicture);
		button = (Button) findViewById(R.id.button);

		ArrayAdapter<CharSequence> adapter = ArrayAdapter.createFromResource(
				this, R.array.dayPrompt, android.R.layout.simple_spinner_item);
		adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		DaySelector.setAdapter(adapter);
		DaySelector.setOnItemSelectedListener(this);

		ArrayAdapter<CharSequence> adapter2 = ArrayAdapter.createFromResource(
				this, R.array.channelPrompt,
				android.R.layout.simple_spinner_item);
		adapter2.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
		ChannelSelector.setAdapter(adapter2);
		ChannelSelector.setOnItemSelectedListener(this);
		button.setOnClickListener(this);

		// array adapter for day spinner

	}

	public void onClick(View arg0) {

		switchscreen = (ViewSwitcher) findViewById(R.id.vwSwitch);
		if (arg0 == button) {
			try {
				result = tvListingString(tvListingURL + day);
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}

			response.setText("");
			// Creates new instance of XML Parser then parses the required XML.
			XmlParser myXMLParser;
			myXMLParser = new XmlParser();
			LinkedList<Widget> alist = myXMLParser.parseWidgetXMLString(result);

			switchscreen.showNext();

			Intent i = getBaseContext().getPackageManager()
					.getLaunchIntentForPackage(
							getBaseContext().getPackageName());
			i.addFlags(Intent.FLAG_ACTIVITY_CLEAR_TOP);

		}
	}

	// Changes Listing URL to a specific channel depending on what is selected
	// on the spinner
	public void onItemSelected(AdapterView<?> arg0, View arg1, int arg2,
			long arg3) {

		if (arg0 == ChannelSelector) {

			String text = (String) ChannelSelector.getSelectedItem();

			if (text.equals("BBC 1")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=bbc1_scotland&day=";
				img.setImageResource(R.drawable.bbc1);

			}

			else if (text.equals("BBC 2")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=bbc2_scotland&day=";
				img.setImageResource(R.drawable.bbc_two);
			}

			else if (text.equals("BBC 3")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=bbc3&day=";
				img.setImageResource(R.drawable.bbc_three);
			}

			else if (text.equals("Sky Action")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=sky_movies_action_thriller&day=";
				img.setImageResource(R.drawable.action);

			}

			else if (text.equals("Uk Gold")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=uk_gold&day=";
				img.setImageResource(R.drawable.uk_gold);

			}

			else if (text.equals("Boomerang")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=boomerang&day=";
				img.setImageResource(R.drawable.boomerang);

			}

			else if (text.equals("Eurosport")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=british_eurosport&day=";
				img.setImageResource(R.drawable.british_eurosport);

			}

			else if (text.equals("Extreme Sports")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=extreme_sports&day=";
				img.setImageResource(R.drawable.extreme);

			}

			else if (text.equals("Challenge")) {
				tvListingURL = "http://bleb.org/tv/data/rss.php?ch=challenge&day=";
				img.setImageResource(R.drawable.challenge);

			}

		}

		try {
			result = tvListingString(tvListingURL + day);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

		response.setText("");
		// Creates new instance of XML Parser then parses the required XML.
		XmlParser myXMLParser;
		myXMLParser = new XmlParser();
		LinkedList<Widget> alist = myXMLParser.parseWidgetXMLString(result);

		if (alist != null) {

			int counter = 0;
			for (Widget element : alist) {
				if (counter == 0) {
					counter++;
				}

				else {
					response.append(element.getTitle() + "\n");
					response.append(element.getDescription() + "\n");
					response.append("--------------------------------------------------------------"
							+ "\n");

					counter++;
				}
			}
		}
		if (arg0 == DaySelector) {
			String text2 = (String) DaySelector.getSelectedItem();

			if (text2.equals("Today")) {
				day = "0";
			}

			else if (text2.equals("Tommorow")) {
				day = "1";
			}

			else if (text2.equals("Next 2 Days")) {
				day = "2";

			}

			else if (text2.equals("Next 3 Days")) {
				day = "3";

			}

			else if (text2.equals("Next 4 Days")) {
				day = "4";
			}

			else if (text2.equals("Next 5 Days")) {
				day = "5";
			}

			else if (text2.equals("Next 6 Days")) {
				day = "6";
			}

			else if (text2.equals("Yesterday")) {
				day = "-1";
			}
		}

	}

	private static String tvListingString(String urlString) throws IOException {
		String result = "";
		InputStream anInStream = null;
		int response = -1;
		URL url = new URL(urlString);
		URLConnection conn = url.openConnection();
		if (!(conn instanceof HttpURLConnection))
			throw new IOException("Not an HTTP connection");
		try {
			HttpURLConnection httpConn = (HttpURLConnection) conn;
			httpConn.setAllowUserInteraction(false);
			httpConn.setInstanceFollowRedirects(true);
			httpConn.setRequestMethod("GET");
			httpConn.connect();
			response = httpConn.getResponseCode();
			if (response == HttpURLConnection.HTTP_OK) {
				anInStream = httpConn.getInputStream();
				InputStreamReader in = new InputStreamReader(anInStream);
				BufferedReader bin = new BufferedReader(in);
				// result = bin.readLine();
				// Input is over multiple lines
				String line = new String();
				while (((line = bin.readLine())) != null) {
					result = result + "\n" + line;
				}
			}
		} catch (Exception ex) {
			throw new IOException("Error connecting");
		}
		return result;
	}

	public void onNothingSelected(AdapterView<?> arg0) {
		// TODO Auto-generated method stub

	}
}

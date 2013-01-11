#
#	Example Linux C program with BASH script driver
#	James Brooks  1/9/2013
#   Written as an assignment for a Unix class at OIT Spring 2012
#
#	The bash script, searchcraigs, is a driver for the program cscrape.
# 	It passes arguments to cscrape facillitating search of portland.craigslist.org,
#	finding items for sale.  A list of links for any items found is returned.
#
#	A history of searh items found and search terms used are kept in the
#   	user's home directory.  These filenames are prepended a period to make
#   	them hidden from normal listing calls.  The filenames all contain the
#   	user name and the word cscrape.  
#
#   	The history file keeps track of searches made for the last
#   	last thirty days and so does not report any previously found
#   	links.  This feature can be overridden by specifying -H
#   	on the command line which creates a new history file.
#
#   	The last search terms specified are stored so searching for
#   	the same items repeatedly allows execution without search terms
#	
#	Searching looks for terms in both title and ad content information.
#   
#   	The link results of the last search returning new items are stored
#   	in a file in the home directory file.	
#	
#   	The -Q flag turns off program advisories
#
#	command line example  ./searchcraigs [-H] [-Q] [<searchterm> <searchterm> ...]
#
#	example output
#
#	./searchcraigs roomba scooba
#	http://portland.craigslist.org/mlt/hsd/3058208477.html IROBOT ROOMBA VACUUM
#	http://portland.craigslist.org/clk/mat/3017818763.html BRAND NEW! Factory sealed iRobot Roomba 780
#	http://portland.craigslist.org/mlt/tag/3064158484.html Voice Activated R2D2
#	http://portland.craigslist.org/clk/ele/3045754853.html iRobot Roomba Scooba Repair
#	http://portland.craigslist.org/clk/ele/3045784807.html I WANT YOUR BROKEN ROOMBA IROBOT SCOOBA
#	http://corvallis.craigslist.org/hsh/3061920702.html iRobot Roomba 440 - $75
#	http://portland.craigslist.org/mlt/for/3062044379.html I Robot Scooba
#	http://portland.craigslist.org/clk/hsh/3058015297.html iROBOT SCOOBA OR TRADE
#
#
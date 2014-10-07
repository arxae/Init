Init
====

Template based directory initialiser

Quickstart
----------
Usage:

* "Init --help" shows the help page
* "Init -t express" extracts the express template
* "Init -t express -s extra" extracts the express extra subtemplate
* "Init -t express -v" extracts the express template showing verbose output
* "Init appname -t express" extracts the express template with appname as the namechange (see Template lua options for more information)

###Creating a template:

In the init root folder, create a "templates" folder. Inside this folder, create a folder with the name of the template (for example, "express"). Inside this folder you place the zip archives of all the subtemplates.

When init is launched witouth a subtemplate parameter, the archive "default.zip" will be extracted. So be sure the name the default archive like this.

###Template lua options
In your template folder, you can create a template.lua file. This file gets executed just before the extraction begins. In here you can override certain options. So far this is only the appname.

By default,  &lt;&lt;&lt;appname&gt;&gt;&gt; string is present inside a file, Init will change this string to either the folder where it's extracting to, or the appname is the proper argument is given

The string that Init looks for can be overriden in the template.lua file using

    template.appNameMacro = "|>|appnamehere|<|"
    
Now, wherever |>|appnamehere|<| in the template appears will be replaced by the appname determined by init

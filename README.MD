# Underground Injection Control ETL To EPA XML Tool

This tool is run on an ArcSDE database containing facilities and wells. It generates
an XML document that can be submitted to the EPA.

### Usage

1. Open a command window in a folder containing `uic-etl.exe`.
1. Execute `uic-etl.exe -h` or `uic-etl.exe --help` from the command line to see the options and help text.
1. Execute `uic-etl.exe -c c:\path\to\db.sde`
1. Visit the [Submission Log](https://docs.google.com/spreadsheets/d/1jeYvLWq7XFmDgKayO7ZFyNuqzkRnDS9Wz0ozwquTErA/) to see warnings and errors for the current run.
  - The current run will be in a tab with the current date and time.

#### Options
```
λ uic-etl.exe -h
Usage: uicetl [OPTIONS]+

Options:
  -c, --connection=VALUE     REQUIRED. the path to the .sde connection file for
                               the UTEQ database. eg: c:\udeq.sde
  -o, --output=VALUE         the location to save the output of
                               this tool. eg: c:\. Defaults to current
                               working directory.
  -v                         increase the debug message verbosity.
  -h, --help                 show this message and exit
```

#### Example

`uic-etl.exe -c C:\sde\UDEQ.sde -o c:\udeq.xml -v`

### Reference Items

- [UIC XML v2 Schema](http://www.exchangenetwork.net/schema/UIC/2/UIC_SampleXML_v2.0.xml)
- [Submission Log](https://docs.google.com/spreadsheets/d/1jeYvLWq7XFmDgKayO7ZFyNuqzkRnDS9Wz0ozwquTErA/)
- [Production Submission Url](https://enservices.epa.gov/Login.aspx)
- [Test Submission Url](https://enservicestest.epacdxnode.net/login.aspx)

### Submission Steps

- Run the tool
- Veryify the warnings in the [Submission Log](https://docs.google.com/spreadsheets/d/1jeYvLWq7XFmDgKayO7ZFyNuqzkRnDS9Wz0ozwquTErA/)
- Compress the xml file to zip
- Login to the [cdxnode](https://enservices.epa.gov/Login.aspx)
- Click on `Exchange Network Service`
- Select `Send Information`
- Search for `UIC`
- Click `Send Info`
- Attach and Upload File
- Fill out metadata
- Add contact email
- Submit

# GS1-128-Barcoder
A C# class object to generate a GS1 128 barcode.  

Currently only setup for a single application identifier at a time.  
Produces a GS1 128 printable barcode as:
```
({Application Identifier}){Extension Digit}{GS1 Company Prefix}{Serial Number}{Check Digit}

(00) 0 1234567 000000001 5

(00)012345670000000015
```
The encoded GS1 128 barcode is as follows
```
{char(205)}{char(202)}{Encoded barcode - not including parenthesis}{C128 Check Digit}{char(206)}

Í Ê Â!7McÂÂÂÂ/ ` Î

ÍÊÂ!7McÂÂÂÂ/`Î
```

## Usage
Initialise a new BarcodeGS1128 object.
```csharp
var barcode = new Barcode.BarcodeGS1128 {
    ApplicationIdentifier = "00",
    ExtensionDigit = "0",
    GS1CompanyPrefix = "1234567",
    SerialNumber = "000000001"
};
```

For more information on GS1 128 please visit https://www.gs1-128.info/

<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
  
  <xsl:output method="xml" version="1.0" encoding="UTF-8" />

  <!-- From Theme: -->
  <!-- Header colours and text settings -->
  <xsl:variable name="headerBackCol" select="'orange'" />
  <xsl:variable name="headerTextCol" select="'black'" />
  <xsl:variable name="headerTextSize" select="'40pt'" />
  <xsl:variable name="headerFontFamily" select="'Helvetica'" />

  <!-- Text area colours and text settings -->
  <xsl:variable name="bodyBackCol" select="'black'" />
  <xsl:variable name="bodyTextCol" select="'white'" />
  <xsl:variable name="bodyTextSize" select="'12pt'" />
  <xsl:variable name="bodyFontFamily" select="'Helvetica'" />

  <xsl:template match="/page">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        <!--A4 Portrait-->
        <fo:simple-page-master master-name="FrontMaster" page-width="230mm" page-height="317mm">
          <fo:region-body region-name="front" />
        </fo:simple-page-master>
        <fo:simple-page-master master-name="BackMaster" page-width="230mm" page-height="317mm">
          <fo:region-body region-name="back" />
        </fo:simple-page-master>

        <fo:page-sequence-master master-name="document">
          <fo:repeatable-page-master-alternatives>
            <fo:conditional-page-master-reference page-position="any" odd-or-even="odd" master-reference="FrontMaster" />
            <fo:conditional-page-master-reference page-position="any" odd-or-even="even" master-reference="BackMaster" />
          </fo:repeatable-page-master-alternatives>
        </fo:page-sequence-master>
        
      </fo:layout-master-set>

      <fo:page-sequence master-reference="FrontMaster">
        <fo:flow flow-name="front">
          <!--This is the template containing the main body layout.-->
          <xsl:call-template name="pageContentFront" />
        </fo:flow>
      </fo:page-sequence>
      <fo:page-sequence master-reference="BackMaster">
        <fo:flow flow-name="back">
          <xsl:call-template name="pageContentBack" />
        </fo:flow>
      </fo:page-sequence>
	  
    </fo:root>
  </xsl:template>

  <!-- page templates-->
  <xsl:template name="pageContentFront">
    
    <!-- Header -->
    <fo:block-container position="fixed" top="10mm" left="10mm" height="18mm" width="210mm" display-align="center" text-align="center" letter-spacing="-0.02em">
      <xsl:attribute name="background-color">
        <xsl:value-of select="$headerBackCol"/>
      </xsl:attribute>
      <xsl:attribute name="font-family">
        <xsl:value-of select="$headerFontFamily"/>
      </xsl:attribute>
      <xsl:attribute name="font-size">
        <xsl:value-of select="$headerTextSize"/>
      </xsl:attribute>
      <xsl:attribute name="line-height">
        <xsl:value-of select="$headerTextSize"/>
      </xsl:attribute>
      <xsl:attribute name="color">
        <xsl:value-of select="$headerTextCol"/>
      </xsl:attribute>
      <fo:block margin-left="0mm" margin-right="0mm" margin-top="0mm" margin-bottom="0mm"><xsl:copy-of select="heading/node()"/></fo:block>
    </fo:block-container>
    
    <!-- Body content -->
    <fo:block-container position="fixed" top="37mm" left="10mm" height="86mm" width="210mm" display-align="before" text-align="left" letter-spacing="0em" linefeed-treatment="preserve">
      <xsl:attribute name="background-color">
        <xsl:value-of select="$bodyBackCol"/>
      </xsl:attribute>
      <xsl:attribute name="font-family">
        <xsl:value-of select="$bodyFontFamily"/>
      </xsl:attribute>
      <xsl:attribute name="font-size">
        <xsl:value-of select="$bodyTextSize"/>
      </xsl:attribute>
      <xsl:attribute name="line-height">
        <xsl:value-of select="$bodyTextSize"/>
      </xsl:attribute>
      <xsl:attribute name="color">
        <xsl:value-of select="$bodyTextCol"/>
      </xsl:attribute>
      <fo:block margin-left="6mm" margin-right="6mm" margin-top="8mm" margin-bottom="8mm"><xsl:copy-of select="body/node()"/></fo:block>
    </fo:block-container>
    
    <!-- Image -->
    <fo:block-container position="fixed" bottom="23mm" right="10mm" height="20mm" width="210mm" text-align="center" display-align="after">
      <fo:block>
        <fo:external-graphic scaling="uniform" content-height="20mm">
          <xsl:attribute name="src">url('<xsl:value-of select="logo"/>')</xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>

    <!-- Crop marks -->
    <fo:block-container position="fixed" top="0mm" left="0mm" height="10mm" width="100%" background-color="transparent" border-bottom="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    <fo:block-container position="fixed" top="0mm" left="0mm" height="100%" width="10mm" background-color="transparent" border-right="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    <fo:block-container position="fixed" top="0mm" right="0mm" height="100%" width="10mm" background-color="transparent" border-left="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    <fo:block-container position="fixed" bottom="0mm" left="0mm" height="10mm" width="100%" background-color="transparent" border-top="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    
  </xsl:template>

  <xsl:template name="pageContentBack">
    <!-- Body content (on back)-->
    <fo:block-container position="fixed" top="43mm" left="10mm" bottom="188mm" right="127mm" display-align="before" text-align="left" letter-spacing="0em" linefeed-treatment="preserve">
      <xsl:attribute name="background-color">
        <xsl:value-of select="$bodyBackCol"/>
      </xsl:attribute>
      <xsl:attribute name="font-family">
        <xsl:value-of select="$bodyFontFamily"/>
      </xsl:attribute>
      <xsl:attribute name="font-size">
        <xsl:value-of select="$bodyTextSize"/>
      </xsl:attribute>
      <xsl:attribute name="line-height">
        <xsl:value-of select="$bodyTextSize"/>
      </xsl:attribute>
      <xsl:attribute name="color">
        <xsl:value-of select="$bodyTextCol"/>
      </xsl:attribute>
      <fo:block margin-left="6mm" margin-right="6mm" margin-top="8mm" margin-bottom="8mm">
        <xsl:copy-of select="body2/node()"/>
      </fo:block>
    </fo:block-container>
    
    <!-- Logo on back-->
    <fo:block-container position="fixed" bottom="23mm" right="10mm" height="20mm" width="115mm" text-align="center" display-align="after">
      <fo:block>
        <fo:external-graphic scaling="uniform" content-height="20mm">
          <xsl:attribute name="src">
            url('<xsl:value-of select="logo"/>')
          </xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>

    <!-- Crop marks -->
    <fo:block-container position="fixed" top="0mm" left="0mm" height="10mm" width="100%" background-color="transparent" border-bottom="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    <fo:block-container position="fixed" top="0mm" left="0mm" height="100%" width="10mm" background-color="transparent" border-right="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    <fo:block-container position="fixed" top="0mm" right="0mm" height="100%" width="10mm" background-color="transparent" border-left="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
    <fo:block-container position="fixed" bottom="0mm" left="0mm" height="10mm" width="100%" background-color="transparent" border-top="thin dashed black" linefeed-treatment="preserve">
      <fo:block>
      </fo:block>
    </fo:block-container>
  </xsl:template>

</xsl:stylesheet>

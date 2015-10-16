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
        <!--A5 Landscape -->
        <fo:simple-page-master master-name="FrontMaster" page-width="210mm" page-height="148mm">
          <fo:region-body region-name="front" />
        </fo:simple-page-master>
        <fo:simple-page-master master-name="BackMaster" page-width="210mm" page-height="148mm">
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
    
    <!-- Hero Image -->
    <fo:block-container position="fixed" top="0mm" left="0mm" bottom="0mm" right="0mm" display-align="center" text-align="center">
      <fo:block margin="0mm">
        <fo:external-graphic scaling="uniform" content-width="210mm">
          <xsl:attribute name="src">
            url('<xsl:value-of select="hero_image"/>')
          </xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>

  </xsl:template>

  <xsl:template name="pageContentBack">

    <!-- Hero image -->
    <fo:block-container position="fixed" top="0mm" left="0mm" bottom="0mm" width="128mm" display-align="center" text-align="center">
      <fo:block>
        <fo:external-graphic scaling="uniform" content-width="128mm">
          <xsl:attribute name="src">
            url('<xsl:value-of select="back_hero_image"/>')
          </xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>
    
    <!-- Address block -->
    <fo:block-container position="fixed" top="74mm" right="14mm" width="90mm" height="55mm" background-color="#D7D7D7" display-align="before" text-align="left" font-family="Helvetica" font-size="12pt" line-height="12pt" letter-spacing="0em" color="black">
      <fo:block margin-left="6mm" margin-right="6mm" margin-top="8mm" margin-bottom="8mm">
        <fo:block>Address line 1</fo:block>
        <fo:block>Address line 2</fo:block>
        <fo:block>City / Town</fo:block>
        <fo:block>AA1 1AA</fo:block>
        <fo:block>United Kingdom</fo:block>
      </fo:block>
    </fo:block-container>

    <!-- Postage block -->
    <fo:block-container position="fixed" top="0mm" right="0mm" height="40mm" width="75mm" text-align="center" display-align="center" background-color="#505050" color="white" font-size="12pt" font-family="Helvetica">
      <fo:block>Postage here</fo:block>
    </fo:block-container>

  </xsl:template>

</xsl:stylesheet>

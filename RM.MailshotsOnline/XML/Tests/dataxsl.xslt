<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
  
  <xsl:output method="xml" version="1.0" encoding="UTF-8" />

  <xsl:template match="/page">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        <!--A4 Portrait-->
        <fo:simple-page-master master-name="A4Front" page-width="210mm" page-height="297mm">
          <fo:region-body region-name="front" />
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A4Back" page-width="210mm" page-height="297mm">
          <fo:region-body region-name="back" />
        </fo:simple-page-master>

        <fo:page-sequence-master master-name="document">
          <fo:repeatable-page-master-alternatives>
            <fo:conditional-page-master-reference page-position="first" master-reference="A4Front" />
            <fo:conditional-page-master-reference page-position="rest" master-reference="A4Back" />
          </fo:repeatable-page-master-alternatives>
        </fo:page-sequence-master>
        
      </fo:layout-master-set>

      <fo:page-sequence master-reference="A4Front">
        <fo:flow flow-name="front">
          <!--This is the template containing the main body layout.-->
          <xsl:call-template name="pageContentFront" />
        </fo:flow>
      </fo:page-sequence>
      <!--<fo:page-sequence master-reference="A4Back">
        <fo:flow flow-name="back">
          <xsl:call-template name="pageContentBack" />
        </fo:flow>
      </fo:page-sequence>-->
	  
    </fo:root>
  </xsl:template>

  <xsl:template name="pageContentFront">
    <!-- Header -->
    <fo:block-container osition="fixed" top="10mm" left="0mm" height="24mm" width="100%" background-color="orange" display-align="center" text-align="center" font-family="Helvetica" font-size="40pt" line-height="40pt" color="black" letter-spacing="-0.02em">
      <fo:block margin-left="0mm" margin-right="0mm" margin-top="0mm" margin-bottom="0mm"><xsl:copy-of select="heading/node()"/></fo:block>
    </fo:block-container>
    
    <!-- Body content -->
    <fo:block-container position="fixed" top="40mm" left="0mm" height="70mm" width="100%" background-color="black" display-align="before" text-align="left" font-family="Helvetica" font-size="12pt" line-height="12pt" letter-spacing="0em" color="white">
      <fo:block margin-left="0mm" margin-right="0mm" margin-top="0mm" margin-bottom="0mm"><xsl:copy-of select="body/node()"/></fo:block>
    </fo:block-container>
    
    <!-- Image -->
    <fo:block-container position="fixed" bottom="40mm" right="0mm" height="16mm" width="100%" text-align="center" display-align="after">
      <fo:block>
        <fo:external-graphic scaling="uniform" content-height="16mm">
          <xsl:attribute name="src">url('<xsl:value-of select="logo"/>')</xsl:attribute>
        </fo:external-graphic>
      </fo:block>
    </fo:block-container>
  </xsl:template>

  <!--<xsl:template name="pageContentBack">
  </xsl:template>-->

</xsl:stylesheet>

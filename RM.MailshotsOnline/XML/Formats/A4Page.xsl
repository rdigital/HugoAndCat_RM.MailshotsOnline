<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
  <!--External files listed here-->
  <xsl:include href="../Templates/A4Page-Demo-Template.xsl" />
  
  <xsl:output method="xml" version="1.0" encoding="UTF-8" />

  <xsl:template match="/ticket">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        <!--A4 Portrait-->
        <fo:simple-page-master master-name="A4-Front" page-width="210mm" page-height="297mm">
          <fo:region-body region-name="front" />
        </fo:simple-page-master>
        <fo:simple-page-master master-name="A4-Back" page-width="210mm" page-height="297mm">
          <fo:region-body region-name="back" />
        </fo:simple-page-master>
      </fo:layout-master-set>

      <xsl:apply-templates/>
	  
    </fo:root>
  </xsl:template>

  <xsl:template match="front">
    <fo:page-sequence master-reference="A4-Front">
      <fo:flow flow-name="front">
        <!--This is the template containing the main body layout.-->
        <xsl:call-template name="pageContent" />
      </fo:flow>
    </fo:page-sequence>
  </xsl:template>

  <xsl:template match="back">
    <fo:page-sequence master-reference="A4-Back">
      <fo:flow flow-name="back">
        <!--This is the template containing the main body layout.-->
        <xsl:call-template name="pageContent" />
      </fo:flow>
    </fo:page-sequence>
  </xsl:template>

</xsl:stylesheet>

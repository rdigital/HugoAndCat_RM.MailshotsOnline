<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
  <xsl:output method="xml" version="1.0" encoding="UTF-8" />

  <xsl:template match="/ticket">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        <!--A5 Landscape -->
        <fo:simple-page-master master-name="Postcard-Front" page-width="210mm" page-height="148mm">
          <fo:region-body region-name="front" />
        </fo:simple-page-master>
        <fo:simple-page-master master-name="Postcard-Back" page-width="210mm" page-height="148mm">
          <fo:region-body region-name="back" />
        </fo:simple-page-master>
      </fo:layout-master-set>

      <fo:page-sequence master-reference="Postcard-Front">
        <fo:flow flow-name="front">
          <!--This is the template containing the main body layout.-->
          <xsl:call-template name="pageContent" />
        </fo:flow>
      </fo:page-sequence>
      <fo:page-sequence master-reference="Postcard-Back">
        <fo:flow flow-name="back">
          <!--This is the template containing the main body layout.-->
          <xsl:call-template name="pageContent" />
        </fo:flow>
      </fo:page-sequence>
    </fo:root>
  </xsl:template>

</xsl:stylesheet>

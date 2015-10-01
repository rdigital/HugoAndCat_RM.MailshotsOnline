<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
  
  <xsl:output method="xml" version="1.0" encoding="UTF-8" />

  <!-- From Theme: -->
  <!-- Header colours and text settings -->
  <xsl:variable name="headerBackCol" select="'#da202a'" />
  <xsl:variable name="headerTextCol" select="'#FFF'" />
  <xsl:variable name="headerTextSize" select="'40pt'" />
  <xsl:variable name="headerFontFamily" select="'Helvetica'" />

  <!-- Text area colours and text settings -->
  <xsl:variable name="bodyBackCol" select="'white'" />
  <xsl:variable name="bodyTextCol" select="'black'" />
  <xsl:variable name="bodyTextSize" select="'12pt'" />
  <xsl:variable name="bodyFontFamily" select="'Helvetica'" />

  <xsl:template match="/invoice">
    <!--This sets up pages, dimensions, backgrounds, headers and footers. Background images can also be applied within this section.-->
    <fo:root>
      <fo:layout-master-set>
        <!--A4 Portrait-->
        <fo:simple-page-master master-name="FrontMaster" page-width="210mm" page-height="297mm">
          <fo:region-body region-name="front" />
        </fo:simple-page-master>

        <fo:page-sequence-master master-name="document">
          <fo:repeatable-page-master-alternatives>
            <fo:conditional-page-master-reference page-position="any" odd-or-even="odd" master-reference="FrontMaster" />
          </fo:repeatable-page-master-alternatives>
        </fo:page-sequence-master>
        
      </fo:layout-master-set>

      <fo:page-sequence master-reference="FrontMaster">
        <fo:flow flow-name="front">
          <!--This is the template containing the main body layout.-->
          <xsl:call-template name="pageContentFront" />
        </fo:flow>
      </fo:page-sequence>
	  
    </fo:root>
  </xsl:template>

  <!-- page templates-->
  <xsl:template name="pageContentFront">
    
    <!-- Header -->
    <fo:block-container position="fixed" top="0mm" left="0mm" height="18mm" width="100%" display-align="center" text-align="center" letter-spacing="-0.02em">
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
      <fo:block margin-left="0mm" margin-right="0mm" margin-top="0mm" margin-bottom="0mm">Royal Mail - Mailshots Online Invoice</fo:block>
    </fo:block-container>
    
    <!-- Addressee -->
    <xsl:call-template name="textArea">
      <xsl:with-param name="top" select="'20mm'" />
      <xsl:with-param name="left" select="'0mm'" />
      <xsl:with-param name="width" select="'48%'" />
      <xsl:with-param name="height" select="'100mm'" />
      <xsl:with-param name="content">
        <fo:block>Billed to:</fo:block>
        <xsl:value-of select="billing-address/node()"/>
      </xsl:with-param>
    </xsl:call-template>
    
    <!-- Invoice details: -->
    <xsl:call-template name="textArea">
      <xsl:with-param name="top" select="'20mm'" />
      <xsl:with-param name="width" select="'48%'" />
      <xsl:with-param name="right" select="'0mm'" />
      <xsl:with-param name="height" select="'100mm'" />
      <xsl:with-param name="content">
        <fo:table>
          <fo:table-body>
            <fo:table-row>
              <fo:table-cell>
                <fo:block font-weight="bold">Order number:</fo:block>
              </fo:table-cell>
              <fo:table-cell>
                <fo:block>
                  <xsl:value-of select="order-number"/>
                </fo:block>
              </fo:table-cell>
            </fo:table-row>
            <fo:table-row>
              <fo:table-cell>
                <fo:block font-weight="bold">Order date:</fo:block>
              </fo:table-cell>
              <fo:table-cell>
                <fo:block>
                  <xsl:value-of select="order-date"/>
                </fo:block>
              </fo:table-cell>
            </fo:table-row>
            <fo:table-row>
              <fo:table-cell>
                <fo:block font-weight="bold">Payment date:</fo:block>
              </fo:table-cell>
              <fo:table-cell>
                <fo:block>
                  <xsl:value-of select="paid-date"/>
                </fo:block>
              </fo:table-cell>
            </fo:table-row>
          </fo:table-body>
        </fo:table>
      </xsl:with-param>
    </xsl:call-template>
    
    <!-- Line items -->
    
  </xsl:template>

  <xsl:template name="textArea">
    <xsl:param name="content" select="''" />
    <xsl:param name="top" select="''" />
    <xsl:param name="left" select="''" />
    <xsl:param name="height" select="''" />
    <xsl:param name="width" select="''" />
    <xsl:param name="right" select="''" />
    <xsl:param name="bottom" select="''" />
    <fo:block-container position="fixed" display-align="before" text-align="left" letter-spacing="0em" linefeed-treatment="preserve">
      <xsl:if test="$top != ''">
        <xsl:attribute name="top">
          <xsl:value-of select="$top"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$left != ''">
        <xsl:attribute name="left">
          <xsl:value-of select="$left"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$height != ''">
        <xsl:attribute name="height">
          <xsl:value-of select="$height"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$width != ''">
        <xsl:attribute name="width">
          <xsl:value-of select="$width"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$right != ''">
        <xsl:attribute name="right">
          <xsl:value-of select="$right"/>
        </xsl:attribute>
      </xsl:if>
      <xsl:if test="$bottom != ''">
        <xsl:attribute name="bottom">
          <xsl:value-of select="$bottom"/>
        </xsl:attribute>
      </xsl:if>
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
        <xsl:copy-of select="$content"/>
      </fo:block>
    </fo:block-container>
  </xsl:template>

</xsl:stylesheet>

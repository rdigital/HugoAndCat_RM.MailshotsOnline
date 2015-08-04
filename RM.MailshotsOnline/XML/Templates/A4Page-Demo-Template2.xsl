<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
  <!--External files listed here-->
  <xsl:include href="../Common/styles.xsl" />
  <xsl:include href="../Common/modules.xsl" />

	<xsl:output method="xml" version="1.0" encoding="UTF-8" />
	
	<!--Content for page goes here. Templates for page elements are called and parameter values injected.-->
	<!--These templates are held in common/modules.xsl so that they can easily be shared between similar templates.-->
	<xsl:template name="pageContent">
		
		<!--Page Header-->
		<xsl:call-template name="header">
			<!--Positional co-ordinates-->
			<xsl:with-param name="head_Top" select="'10mm'" />
			<xsl:with-param name="head_Left" select="'0mm'" />
			<!--Dimensions-->
			<xsl:with-param name="head_Height" select="'24mm'" />
			<xsl:with-param name="head_Width" select="'100%'" />
			<xsl:with-param name="head_BackgroundColor" select="'orange'" />
			<!--Vertical alignment for content/text-->
			<xsl:with-param name="head_DisplayAlign" select="'center'" />
			<xsl:with-param name="head_TagMarginLeft" select="'0mm'" />
			<xsl:with-param name="head_TagMarginRight" select="'0mm'" />
			<xsl:with-param name="head_TagMarginTop" select="'0mm'" />
			<xsl:with-param name="head_TagMarginBottom" select="'0mm'" />
			<xsl:with-param name="head_TextAlign" select="'center'" />
			<xsl:with-param name="head_FontFamily" select="'Helvetica'" />
			<xsl:with-param name="head_FontSize" select="'40pt'" />
			<!--Similar behaviour to leading-->
			<xsl:with-param name="head_LineHeight" select="'40pt'" />
			<!--Tracking-->
			<xsl:with-param name="head_LetterSpacing" select="'-0.02em'" />
			<!--Content/text colour-->
			<xsl:with-param name="head_Color" select="'black'" />
		</xsl:call-template>

		<!--Main Copy Text-->
		<xsl:call-template name="mainCopy">
			<xsl:with-param name="mainC_Top" select="'40mm'" />
			<xsl:with-param name="mainC_Left" select="'0mm'" />
			<xsl:with-param name="mainC_Height" select="'70mm'" />
			<xsl:with-param name="mainC_Width" select="'100%'" />
			<xsl:with-param name="mainC_BackgroundColor" select="'black'" />
			<xsl:with-param name="mainC_DisplayAlign" select="'before'" />
			<xsl:with-param name="mainC_TagMarginLeft" select="'5mm'" />
			<xsl:with-param name="mainC_TagMarginRight" select="'5mm'" />
			<xsl:with-param name="mainC_TagMarginTop" select="'5mm'" />
			<xsl:with-param name="mainC_TagMarginBottom" select="'2mm'" />
			<xsl:with-param name="mainC_TextAlign" select="'left'" />
			<xsl:with-param name="mainC_FontFamily" select="'Helvetica'" />
			<xsl:with-param name="mainC_FontSize" select="'12pt'" />
			<xsl:with-param name="mainC_LineHeight" select="'12pt'" />
			<xsl:with-param name="mainC_LetterSpacing" select="'0em'" />
			<xsl:with-param name="mainC_Color" select="'white'" />
		</xsl:call-template>
		
		<!--Graphic-->
		<!--Graphic selection is powered by the data, rules held in common/modules.xsl-->
		<xsl:call-template name="graphic">
			<xsl:with-param name="graph_Bottom" select="'40mm'" />
			<xsl:with-param name="graph_Right" select="'0mm'" />
			<xsl:with-param name="graph_Height" select="'16mm'" />
			<xsl:with-param name="graph_Width" select="'100%'" />
			<xsl:with-param name="graph_ImgHeight" select="'16mm'" />
			<xsl:with-param name="graph_TextAlign" select="'center'" />
			<xsl:with-param name="graph_DisplayAlign" select="'after'" />
			<xsl:with-param name="graph_Name">
				<xsl:value-of select="graphic" />
			</xsl:with-param>
		</xsl:call-template>

	</xsl:template>
	
</xsl:stylesheet>
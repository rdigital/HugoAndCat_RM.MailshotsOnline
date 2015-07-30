<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" />

	<!--Page Header-->
	<xsl:template name="header">
		<!--All required parameter names are defined here-->
		<xsl:param name="head_Top" />
		<xsl:param name="head_Left" />
		<xsl:param name="head_Height" />
		<xsl:param name="head_Width" />
		<xsl:param name="head_BackgroundColor" />
		<xsl:param name="head_DisplayAlign" />
		<xsl:param name="head_TagMarginLeft" />
		<xsl:param name="head_TagMarginRight" />
		<xsl:param name="head_TagMarginTop" />
		<xsl:param name="head_TagMarginBottom" />
		<xsl:param name="head_TextAlign" />
		<xsl:param name="head_FontFamily" />
		<xsl:param name="head_FontSize" />
		<xsl:param name="head_LineHeight" />
		<xsl:param name="head_LetterSpacing" />
		<xsl:param name="head_Color" />
		<!--A block container can accept positional and dimensional values and can hold single or multiple blocks of content-->
		<!--xsl:use-attribute-sets allows a set of pre-defined attributes to be easily applied in many places (see global/styles.xsl)-->
		<!--Attribute values are fed through from page.xsl-->
		<fo:block-container position="fixed" xsl:use-attribute-sets="border">
			<!--Positional co-ordinates-->
			<xsl:attribute name="top">
				<xsl:value-of select="$head_Top" />
			</xsl:attribute>
			<xsl:attribute name="left">
				<xsl:value-of select="$head_Left" />
			</xsl:attribute>
			<xsl:attribute name="height">
				<xsl:value-of select="$head_Height" />
			</xsl:attribute>
			<!--Dimensions-->
			<xsl:attribute name="width">
				<xsl:value-of select="$head_Width" />
			</xsl:attribute>
			<xsl:attribute name="background-color">
				<xsl:value-of select="$head_BackgroundColor" />
			</xsl:attribute>
			<!--Vertical alignment for content/text-->
			<xsl:attribute name="display-align">
				<xsl:value-of select="$head_DisplayAlign" />
			</xsl:attribute>
			<xsl:attribute name="text-align">
				<xsl:value-of select="$head_TextAlign" />
			</xsl:attribute>
			<xsl:attribute name="font-family">
				<xsl:value-of select="$head_FontFamily" />
			</xsl:attribute>
			<xsl:attribute name="font-size">
				<xsl:value-of select="$head_FontSize" />
			</xsl:attribute>
			<!--Similar behaviour to leading-->
			<xsl:attribute name="line-height">
				<xsl:value-of select="$head_LineHeight" />
			</xsl:attribute>
			<!--Tracking-->
			<xsl:attribute name="letter-spacing">
				<xsl:value-of select="$head_LetterSpacing" />
			</xsl:attribute>
			<!--Content/text colour-->
			<xsl:attribute name="color">
				<xsl:value-of select="$head_Color" />
			</xsl:attribute>
			<!--Content is stored in blocks. Blocks automatically re-size to fit content and are constrained by the parent block-container.-->
			<!--By default multiple blocks stack on top of each other (useful to force line breaks)-->
			<fo:block xsl:use-attribute-sets="border">
				<xsl:attribute name="margin-left">
					<xsl:value-of select="$head_TagMarginLeft" />
				</xsl:attribute>
				<xsl:attribute name="margin-right">
					<xsl:value-of select="$head_TagMarginRight" />
				</xsl:attribute>
				<xsl:attribute name="margin-top">
					<xsl:value-of select="$head_TagMarginTop" />
				</xsl:attribute>
				<xsl:attribute name="margin-bottom">
					<xsl:value-of select="$head_TagMarginBottom" />
				</xsl:attribute>
				<xsl:value-of select="header_text" />
			</fo:block>
		</fo:block-container>
	</xsl:template>

	<!--Main Copy Text-->
	<xsl:template name="mainCopy">
		<xsl:param name="mainC_Top" />
		<xsl:param name="mainC_Left" />
		<xsl:param name="mainC_Height" />
		<xsl:param name="mainC_Width" />
		<xsl:param name="mainC_BackgroundColor" />
		<xsl:param name="mainC_DisplayAlign" />
		<xsl:param name="mainC_TagMarginLeft" />
		<xsl:param name="mainC_TagMarginRight" />
		<xsl:param name="mainC_TagMarginTop" />
		<xsl:param name="mainC_TagMarginBottom" />
		<xsl:param name="mainC_TextAlign" />
		<xsl:param name="mainC_FontFamily" />
		<xsl:param name="mainC_FontSize" />
		<xsl:param name="mainC_LineHeight" />
		<xsl:param name="mainC_LetterSpacing" />
		<xsl:param name="mainC_Color" />
		<!--By default, linefeeds in the XML are ignored. The linefeed-treatment="preserve" will preserve them.-->
		<fo:block-container position="fixed" xsl:use-attribute-sets="border" linefeed-treatment="preserve">
			<xsl:attribute name="top">
				<xsl:value-of select="$mainC_Top" />
			</xsl:attribute>
			<xsl:attribute name="left">
				<xsl:value-of select="$mainC_Left" />
			</xsl:attribute>
			<xsl:attribute name="height">
				<xsl:value-of select="$mainC_Height" />
			</xsl:attribute>
			<xsl:attribute name="width">
				<xsl:value-of select="$mainC_Width" />
			</xsl:attribute>
			<xsl:attribute name="background-color">
				<xsl:value-of select="$mainC_BackgroundColor" />
			</xsl:attribute>
			<xsl:attribute name="display-align">
				<xsl:value-of select="$mainC_DisplayAlign" />
			</xsl:attribute>
			<xsl:attribute name="text-align">
				<xsl:value-of select="$mainC_TextAlign" />
			</xsl:attribute>
			<xsl:attribute name="font-family">
				<xsl:value-of select="$mainC_FontFamily" />
			</xsl:attribute>
			<xsl:attribute name="font-size">
				<xsl:value-of select="$mainC_FontSize" />
			</xsl:attribute>
			<xsl:attribute name="line-height">
				<xsl:value-of select="$mainC_LineHeight" />
			</xsl:attribute>
			<xsl:attribute name="letter-spacing">
				<xsl:value-of select="$mainC_LetterSpacing" />
			</xsl:attribute>
			<xsl:attribute name="color">
				<xsl:value-of select="$mainC_Color" />
			</xsl:attribute>
			<fo:block xsl:use-attribute-sets="border">
				<xsl:attribute name="margin-left">
					<xsl:value-of select="$mainC_TagMarginLeft" />
				</xsl:attribute>
				<xsl:attribute name="margin-right">
					<xsl:value-of select="$mainC_TagMarginRight" />
				</xsl:attribute>
				<xsl:attribute name="margin-top">
					<xsl:value-of select="$mainC_TagMarginTop" />
				</xsl:attribute>
				<xsl:attribute name="margin-bottom">
					<xsl:value-of select="$mainC_TagMarginBottom" />
				</xsl:attribute>
				<xsl:value-of select="main_copy_text" />
			</fo:block>
		</fo:block-container>
	</xsl:template>
	
	<!--Graphic-->
	<xsl:template name="graphic">
		<!--Instead of being anchored to the top/left of the page, the graphic is anchored to bottom/right. Whichever combination makes most sense.-->
		<xsl:param name="graph_Bottom" />
		<xsl:param name="graph_Right" />
		<xsl:param name="graph_Height" />
		<xsl:param name="graph_Width" />
		<xsl:param name="graph_ImgHeight" />
		<xsl:param name="graph_ImgWidth" />
		<xsl:param name="graph_Name" />
		<xsl:param name="graph_TextAlign" />
		<xsl:param name="graph_DisplayAlign" />
		<!--The if statement will ignore the graphic if no value is supplied within the XML-->
		<xsl:if test="graphic != ''">
			<fo:block-container xsl:use-attribute-sets="border" position="fixed">
				<xsl:attribute name="bottom">
					<xsl:value-of select="$graph_Bottom" />
				</xsl:attribute>
				<xsl:attribute name="right">
					<xsl:value-of select="$graph_Right" />
				</xsl:attribute>
				<xsl:attribute name="height">
					<xsl:value-of select="$graph_Height" />
				</xsl:attribute>
				<xsl:attribute name="width">
					<xsl:value-of select="$graph_Width" />
				</xsl:attribute>
				<xsl:attribute name="text-align">
					<xsl:value-of select="$graph_TextAlign" />
				</xsl:attribute>
				<xsl:attribute name="display-align">
					<xsl:value-of select="$graph_DisplayAlign" />
				</xsl:attribute>
				<fo:block xsl:use-attribute-sets="border" margin-top="-0.7mm">
					<!--External graphics are used like this, they must be inside a block.-->
					<fo:external-graphic scaling="uniform">
						<!--Regardless of the original graphic, this will constrain the height and the width adjusts proportionally.-->
						<xsl:attribute name="content-height">
							<xsl:value-of select="$graph_ImgHeight" />
						</xsl:attribute>
						<!--Rules for determining which graphic to include based on value present in the XML.-->
						<!--xsl:choose allows for multiple outcomes.-->
						<xsl:choose>
							<!--The contains/translate functions look for the string in graphic node and convert characters to lower case for a more accurate match.-->
							<xsl:when test="contains(translate(graphic,'aclsy ','ACLSY'),'clays')">
								<xsl:attribute name="src">
									url('templates/global/assets/clays.png')
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="contains(translate(graphic,'CDEGHIKNOT ','cdeghiknot'),'connectedthinking')">
								<xsl:attribute name="src">
									url('templates/global/assets/connectedthinking.png')
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="contains(translate(graphic,'ACMO ','acmo'),'occam')">
								<xsl:attribute name="src">
									url('templates/global/assets/occam.png')
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="contains(translate(graphic,'IMS ','ims'),'sims')">
								<xsl:attribute name="src">
									url('templates/global/assets/sims.png')
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="contains(translate(graphic,'GOPRSU ','goprsu'),'spgroup')">
								<xsl:attribute name="src">
									url('templates/global/assets/spgroup.png')
								</xsl:attribute>
							</xsl:when>
							<xsl:when test="contains(translate(graphic,'EGIOPRSTUV ','egioprstuv'),'stivesgroup')">
								<xsl:attribute name="src">
									url('templates/global/assets/stivesgroup.png')
								</xsl:attribute>
							</xsl:when>
						</xsl:choose>
					</fo:external-graphic>
				</fo:block>
			</fo:block-container>
		</xsl:if>
	</xsl:template>

	
</xsl:stylesheet>
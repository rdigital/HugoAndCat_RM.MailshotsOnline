<?xml version="1.0" encoding="UTF-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:fo="http://www.w3.org/1999/XSL/Format" xmlns:rx="http://www.renderx.com/XSL/Extensions">
	<xsl:output method="xml" version="1.0" encoding="UTF-8" />

	<!--Border (For layout guidance)-->
	<!--Borders are useful for checking where block boundaries lie.-->
	<!--This attribute-set can be added to any block or block-container instances.-->
	<!--All borders can then be enabled/disabled by setting border-width to > 0pt.-->
	<xsl:attribute-set name="border">
		<xsl:attribute name="border-style">solid</xsl:attribute>
		<xsl:attribute name="border-width">0pt</xsl:attribute><!--Set to greater than zero (0.25pt) to turn on borders. REMEMBER TO TURN OFF BEFORE RELEASE!!!-->
		<xsl:attribute name="border-color">red</xsl:attribute>
	</xsl:attribute-set>

	
</xsl:stylesheet>
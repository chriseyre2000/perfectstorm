<!--?xml version="1.0" encoding="utf-8"?-->
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:PerfectStorm="urn:PerfectStorm"
				exclude-result-prefixes="PerfectStorm">
  <xsl:output method="xml" standalone="yes" indent="yes"  />

  <xsl:template match="/" xml:space="default">
<entities>
  <xsl:for-each select="model/entity">
    <entity>
	<xsl:attribute name="id">
	<xsl:value-of select="@name" />
	</xsl:attribute>
	<xsl:attribute name="guid">
	<xsl:value-of select="PerfectStorm:NewGuid()" />
	</xsl:attribute>
  </entity>
    </xsl:for-each>
</entities>
  </xsl:template>
</xsl:stylesheet>
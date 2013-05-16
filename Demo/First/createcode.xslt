<!--?xml version="1.0" encoding="utf-8"?-->
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
				xmlns:PerfectStorm="urn:PerfectStorm"
				>
  <xsl:output method="text" standalone="yes" />
  <xsl:preserve-space elements="*" />
  <xsl:param name="entity"/>
  
  <xsl:template match="/">
<xsl:for-each select="model/entity[@name=$entity]">
public class <xsl:value-of select="@name" />Entity 
{
// LowerCase:<xsl:value-of select="PerfectStorm:LowerCase(@name)" />
// UpperCase:<xsl:value-of select="PerfectStorm:UpperCase(@name)" />
}
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
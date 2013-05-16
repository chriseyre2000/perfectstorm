<!--?xml version="1.0" encoding="utf-8"?-->
<xsl:stylesheet version="1.0" 
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:PerfectStorm="urn:PerfectStorm">
  <xsl:output method="text" standalone="yes" />
  <xsl:preserve-space elements="*" />
  <xsl:template match="/">// Header: <xsl:value-of select="PerfectStorm:GetVariable('CodeGen.Tool')" />
  // Guid:<xsl:value-of select="PerfectStorm:NewGuid()" />
  // 1st named Guid:    <xsl:value-of select="PerfectStorm:NamedGuid('namedGuid')" /> 
  // 2nd named Guid:    <xsl:value-of select="PerfectStorm:NamedGuid('namedGuid')" />
  // Guid as variable:  <xsl:value-of select="PerfectStorm:GetVariable('namedGuid')" />
  // Date and time: <xsl:value-of select="PerfectStorm:GetDateTime()" />
  // Generated File: <xsl:value-of select="PerfectStorm:Filename()" />
  // Was regenerated: <xsl:value-of select="PerfectStorm:FileRegenerated()" />
  <xsl:for-each select="model/entity">
public class <xsl:value-of select="@name" /> {}
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
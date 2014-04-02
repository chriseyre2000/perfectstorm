<!--?xml version="1.0" encoding="utf-8"?-->
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
  <xsl:output method="text" standalone="yes" />
  <xsl:preserve-space elements="*" />
  <xsl:template match="/">REM Generate the code<xsl:for-each select="model/entity">
CodeGen model.CodeGen CreateCode.xslt <xsl:value-of select="@name" />Entity.cs entity <xsl:value-of select="@name" />
    </xsl:for-each>
  </xsl:template>
</xsl:stylesheet>
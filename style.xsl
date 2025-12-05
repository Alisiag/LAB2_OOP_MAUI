<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform">
	<xsl:template match="/">
		<html>
			<head>
				<title>Звіт Спорткомплексу</title>
				<style>
					body { font-family: Arial, sans-serif; }
					table { border-collapse: collapse; width: 100%; margin-top: 20px; }
					th, td { border: 1px solid #ddd; padding: 8px; text-align: left; }
					th { background-color: #4CAF50; color: white; }
					tr:nth-child(even) { background-color: #f2f2f2; }
				</style>
			</head>
			<body>
				<h2>Список Секцій Факультету</h2>
				<table>
					<tr>
						<th>Секція</th>
						<th>Тренер</th>
						<th>Час занять</th>
						<th>Кількість студентів</th>
					</tr>
					<xsl:for-each select="SportComplex/Section">
						<tr>
							<td>
								<xsl:value-of select="@Name"/>
							</td>

							<td>
								<xsl:value-of select="Coach/@Name"/>
							</td>

							<td>
								<xsl:value-of select="@Time"/>
							</td>

							<td>
								<xsl:value-of select="count(Student)"/>
							</td>
						</tr>
					</xsl:for-each>
				</table>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
import lucideStatic from 'lucide-static';
import { writeFileSync } from 'fs';
import { fileURLToPath } from 'url';
import { dirname, join } from 'path';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

// C# string escape
function escapeCSharp(str) {
    return str.replace(/"/g, '""');
}

// Tüm ikonları topla (zaten PascalCase)
const iconEntries = Object.entries(lucideStatic)
    .sort(([a], [b]) => a.localeCompare(b))
    .map(([name, svg]) => {
        // Sadece <svg> tag'indeki width ve height'ı kaldır, shape elementlerindekine dokunma
        const svgWithoutSize = svg.replace(
            /(<svg[^>]*)\s+width="[^"]*"([^>]*)\s+height="[^"]*"/,
            '$1$2'
        ).replace(
            /(<svg[^>]*)\s+height="[^"]*"([^>]*)\s+width="[^"]*"/,
            '$1$2'
        );
        const escapedSvg = escapeCSharp(svgWithoutSize);
        return `        public const string ${name} = @"${escapedSvg}";`;
    });

// DzdIcons.cs içeriği
const content = `namespace DZDDashboard.Client.Theme;

/// <summary>
/// Lucide Icons static SVG collection
/// Generated from lucide-static v0.454.0
/// Total icons: ${iconEntries.length}
/// Usage: DzdIcons.Plus, DzdIcons.ArrowLeft, etc.
/// </summary>
public static class DzdIcons
{
${iconEntries.join('\n')}
}
`;

// Dosyayı yaz
const outputPath = join(__dirname, '..', 'DZDDashboard.Client', 'Theme', 'DzdIcons.cs');
writeFileSync(outputPath, content, 'utf8');

console.log(`✅ Generated ${iconEntries.length} icons to DzdIcons.cs`);

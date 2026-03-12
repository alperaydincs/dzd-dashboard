import { readFileSync, writeFileSync, readdirSync, statSync } from 'fs';
import { join, extname } from 'path';
import { fileURLToPath } from 'url';
import { dirname } from 'path';
import iconMapping from './icon-mapping.js';

const __filename = fileURLToPath(import.meta.url);
const __dirname = dirname(__filename);

const componentDir = join(__dirname, '..', 'DZDDashboard.Client', 'Components');

// Find all .razor files recursively
function findRazorFiles(dir, files = []) {
    const items = readdirSync(dir);
    for (const item of items) {
        const fullPath = join(dir, item);
        const stat = statSync(fullPath);
        if (stat.isDirectory()) {
            findRazorFiles(fullPath, files);
        } else if (extname(item) === '.razor') {
            files.push(fullPath);
        }
    }
    return files;
}

// Replace icons in file
function replaceIconsInFile(filePath) {
    let content = readFileSync(filePath, 'utf8');
    let changed = false;
    
    for (const [oldPath, newName] of Object.entries(iconMapping)) {
        const oldPattern = `DzdIcons.${oldPath}`;
        const newPattern = `DzdIcons.${newName}`;
        
        if (content.includes(oldPattern)) {
            content = content.replaceAll(oldPattern, newPattern);
            changed = true;
        }
    }
    
    if (changed) {
        writeFileSync(filePath, content, 'utf8');
        return true;
    }
    return false;
}

// Process all files
const razorFiles = findRazorFiles(componentDir);
let updatedCount = 0;

for (const file of razorFiles) {
    if (replaceIconsInFile(file)) {
        updatedCount++;
        console.log(`✅ Updated: ${file.replace(componentDir, '')}`);
    }
}

console.log(`\n🎉 Updated ${updatedCount} files with new icon names`);

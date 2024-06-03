const archiver = require('archiver');
const fs = require('fs');
const path = require('path');

const output = fs.createWriteStream(path.join(__dirname, 'vue-element-admin.zip'));
const archive = archiver('zip', { zlib: { level: 9 } });

output.on('close', () => {
    console.log(archive.pointer() + ' total bytes');
    console.log('Zip文件已创建成功。');
});

archive.on('error', (err) => {
    throw err;
});

archive.pipe(output);

// 添加所有文件到zip包，但排除node_modules目录
archive.directory('.', false, { ignore: ['node_modules/**'] });

archive.finalize();

-- Motivation初期データ投入
INSERT INTO motivation (
    motivation_name, 
    description, 
    message, 
    icon, 
    color, 
    display_order,
    last_upd_method_name
) VALUES 
(
    'exciting',
    '興奮・やる気満々の状態',
    '🎮 EXCITEMENT MODE! エネルギー全開でタスクに突撃だ！',
    '🚀',
    '#FF00FF00',
    1,
    'InitialData'
),
(
    'lazy',
    'やる気が出ない、だらけたい状態',
    '😴 今日は少しペースダウン...でも少しずつ進もう',
    '😴',
    '#FFFF8000',
    2,
    'InitialData'
),
(
    'postpone',
    '先延ばししたい、後回しにしたい気分',
    '⏰ 「後でやろう」って思ってる？今やっちゃおう！',
    '⏰',
    '#FFFFFF00',
    3,
    'InitialData'
),
(
    'lazy-solved',
    'だらけた状態を克服した',
    '✨ だらけモードから復活！小さな一歩が大きな変化の始まりだ！',
    '✨',
    '#FF00FFAA',
    4,
    'InitialData'
),
(
    'postpone-solved',
    '先延ばし癖を克服した',
    '🎯 先延ばし撃破！行動力が戻ってきた！',
    '🎯',
    '#FF00AAFF',
    5,
    'InitialData'
)
ON CONFLICT (motivation_name) DO NOTHING;